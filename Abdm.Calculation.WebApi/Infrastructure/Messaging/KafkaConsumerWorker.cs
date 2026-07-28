using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Abdm.Calculation.WebApi.Infrastructure.Messaging
{
    /// <summary>
    /// Один IConsumer на топик/группу (Confluent.Kafka client не потокобезопасен).
    /// Параллелизм расчётов задаётся <see cref="KafkaConsumerSettings.ConsumersCount"/>:
    /// сообщения обрабатываются на thread-pool с ограничением степени параллелизма,
    /// а Commit выполняется только на потоке Consume-цикла и только по непрерывной
    /// последовательности оффсетов (чтобы не «перепрыгнуть» ещё не обработанные).
    /// </summary>
    public sealed class KafkaConsumerWorker<TKey, TValue, THandler> : BackgroundService
        where THandler : IKafkaMessageHandler<TKey, TValue>
    {
        private readonly KafkaConsumerSettings _settings;
        private readonly int _workerId;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<KafkaConsumerWorker<TKey, TValue, THandler>> _logger;

        public KafkaConsumerWorker(
            KafkaConsumerSettings settings,
            int workerId,
            IServiceScopeFactory scopeFactory,
            ILogger<KafkaConsumerWorker<TKey, TValue, THandler>> logger)
        {
            _settings = settings;
            _workerId = workerId;
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return Task.Factory.StartNew(
                () => RunConsumeLoop(stoppingToken),
                stoppingToken,
                TaskCreationOptions.LongRunning,
                TaskScheduler.Default);
        }

        private void RunConsumeLoop(CancellationToken stoppingToken)
        {
            var maxConcurrency = Math.Max(1, _settings.ConsumersCount);

            var config = new ConsumerConfig
            {
                BootstrapServers = _settings.BootstrapServers,
                GroupId = _settings.ConsumerGroup,
                AutoOffsetReset = _settings.AutoOffsetReset,
                EnableAutoCommit = false,
                EnableAutoOffsetStore = false,
                // Долгие расчёты не должны вызывать rebalance из‑за отсутствия poll.
                MaxPollIntervalMs = Math.Max(300_000, maxConcurrency * 600_000),
            };

            List<TopicPartition>? assignedPartitions = null;
            var paused = false;
            var offsetTracker = new OffsetTracker();
            var completed = new ConcurrentQueue<ConsumeResult<TKey, string>>();
            var inFlight = 0;

            using var consumer = new ConsumerBuilder<TKey, string>(config)
                .SetErrorHandler((_, error) => _logger.LogError(
                    "Kafka consumer #{WorkerId} error on topic {Topic}: {Reason} (IsFatal: {IsFatal})",
                    _workerId, _settings.Topic, error.Reason, error.IsFatal))
                .SetPartitionsAssignedHandler((_, partitions) =>
                {
                    assignedPartitions = new List<TopicPartition>(partitions);
                    offsetTracker.Clear();
                    _logger.LogInformation(
                        "Kafka consumer #{WorkerId} ({Group}/{Topic}) assigned partitions: [{Partitions}]",
                        _workerId, _settings.ConsumerGroup, _settings.Topic, string.Join(", ", partitions));
                })
                .SetPartitionsRevokedHandler((_, partitions) =>
                {
                    assignedPartitions = null;
                    paused = false;
                    offsetTracker.Clear();
                    _logger.LogInformation(
                        "Kafka consumer #{WorkerId} ({Group}/{Topic}) revoked partitions: [{Partitions}]",
                        _workerId, _settings.ConsumerGroup, _settings.Topic, string.Join(", ", partitions));
                })
                .Build();

            consumer.Subscribe(_settings.Topic);

            _logger.LogInformation(
                "Kafka consumer #{WorkerId} started for topic {Topic}, group {Group}, max concurrency {MaxConcurrency}",
                _workerId, _settings.Topic, _settings.ConsumerGroup, maxConcurrency);

            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    DrainCompleted(consumer, completed, offsetTracker);

                    var busy = Volatile.Read(ref inFlight) >= maxConcurrency;
                    if (busy)
                    {
                        if (!paused && assignedPartitions is { Count: > 0 })
                        {
                            consumer.Pause(assignedPartitions);
                            paused = true;
                        }
                    }
                    else if (paused && assignedPartitions is { Count: > 0 })
                    {
                        consumer.Resume(assignedPartitions);
                        paused = false;
                    }

                    ConsumeResult<TKey, string>? consumeResult;
                    try
                    {
                        consumeResult = consumer.Consume(TimeSpan.FromMilliseconds(100));
                    }
                    catch (ConsumeException ex)
                    {
                        _logger.LogError(
                            ex,
                            "Kafka consumer #{WorkerId} failed to consume a message from {Topic}",
                            _workerId, _settings.Topic);
                        continue;
                    }

                    if (consumeResult is null || consumeResult.IsPartitionEOF)
                    {
                        continue;
                    }

                    // Ожидаемый оффсет фиксируем в порядке Consume (на партиции он монотонный).
                    offsetTracker.OnDispatched(consumeResult);

                    Interlocked.Increment(ref inFlight);
                    var captured = consumeResult;

                    _ = Task.Run(async () =>
                    {
                        try
                        {
                            await HandleMessage(captured, stoppingToken).ConfigureAwait(false);
                            completed.Enqueue(captured);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(
                                ex,
                                "Kafka consumer #{WorkerId} failed to process a message from {Topic} (partition {Partition}, offset {Offset})",
                                _workerId, _settings.Topic, captured.Partition, captured.Offset);
                        }
                        finally
                        {
                            Interlocked.Decrement(ref inFlight);
                        }
                    }, CancellationToken.None);
                }
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
            }
            finally
            {
                var spinUntil = DateTime.UtcNow.AddSeconds(30);
                while (Volatile.Read(ref inFlight) > 0 && DateTime.UtcNow < spinUntil)
                {
                    DrainCompleted(consumer, completed, offsetTracker);
                    Thread.Sleep(50);
                }

                DrainCompleted(consumer, completed, offsetTracker);
                consumer.Close();
                _logger.LogInformation(
                    "Kafka consumer #{WorkerId} stopped for topic {Topic}, group {Group}",
                    _workerId, _settings.Topic, _settings.ConsumerGroup);
            }
        }

        private void DrainCompleted(
            IConsumer<TKey, string> consumer,
            ConcurrentQueue<ConsumeResult<TKey, string>> completed,
            OffsetTracker offsetTracker)
        {
            while (completed.TryDequeue(out var done))
            {
                foreach (var tpo in offsetTracker.MarkProcessed(done))
                {
                    try
                    {
                        consumer.Commit(new[] { tpo });
                    }
                    catch (KafkaException ex)
                    {
                        _logger.LogError(
                            ex,
                            "Kafka consumer #{WorkerId} failed to commit {TopicPartitionOffset}",
                            _workerId, tpo);
                    }
                }
            }
        }

        private async Task HandleMessage(ConsumeResult<TKey, string> consumeResult, CancellationToken cancellationToken)
        {
            TValue? value;
            try
            {
                value = JsonSerializer.Deserialize<TValue>(consumeResult.Message.Value);
            }
            catch (JsonException ex)
            {
                _logger.LogError(
                    ex,
                    "Kafka consumer #{WorkerId} received a message from {Topic} that could not be deserialized into {Type}",
                    _workerId, _settings.Topic, typeof(TValue).Name);
                return;
            }

            if (value is null)
            {
                _logger.LogWarning(
                    "Kafka consumer #{WorkerId} received an empty message from {Topic}",
                    _workerId, _settings.Topic);
                return;
            }

            var typedResult = new ConsumeResult<TKey, TValue>
            {
                Message = new Message<TKey, TValue>
                {
                    Key = consumeResult.Message.Key,
                    Value = value,
                    Timestamp = consumeResult.Message.Timestamp,
                    Headers = consumeResult.Message.Headers,
                },
                TopicPartitionOffset = consumeResult.TopicPartitionOffset,
                IsPartitionEOF = consumeResult.IsPartitionEOF,
            };

            var context = new MessageContext<TKey, TValue>(new[] { typedResult });

            using var scope = _scopeFactory.CreateScope();
            var handler = scope.ServiceProvider.GetRequiredService<THandler>();
            await handler.Handle(value, context, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Коммитит оффсеты только по непрерывному префиксу обработанных сообщений в партиции.
        /// </summary>
        private sealed class OffsetTracker
        {
            private readonly Dictionary<TopicPartition, PartitionOffsets> _partitions = new();

            public void Clear() => _partitions.Clear();

            public void OnDispatched(ConsumeResult<TKey, string> result)
            {
                var tp = result.TopicPartition;
                if (!_partitions.ContainsKey(tp))
                {
                    _partitions[tp] = new PartitionOffsets { NextOffset = result.Offset.Value };
                }
            }

            public IEnumerable<TopicPartitionOffset> MarkProcessed(ConsumeResult<TKey, string> result)
            {
                var tp = result.TopicPartition;
                if (!_partitions.TryGetValue(tp, out var state))
                {
                    state = new PartitionOffsets { NextOffset = result.Offset.Value };
                    _partitions[tp] = state;
                }

                state.Completed.Add(result.Offset.Value);

                var toCommit = new List<TopicPartitionOffset>();
                while (state.Completed.Remove(state.NextOffset))
                {
                    // Сообщение с offset N обработано → следующий fetch с N+1.
                    toCommit.Add(new TopicPartitionOffset(tp, new Offset(state.NextOffset + 1)));
                    state.NextOffset++;
                }

                // Достаточно последнего коммита в непрерывной цепочке.
                if (toCommit.Count <= 1)
                {
                    return toCommit;
                }

                return new[] { toCommit[^1] };
            }

            private sealed class PartitionOffsets
            {
                public long NextOffset;
                public HashSet<long> Completed { get; } = new();
            }
        }
    }
}
