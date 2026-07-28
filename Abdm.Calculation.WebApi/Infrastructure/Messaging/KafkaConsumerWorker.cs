using System;
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
    /// Один экземпляр консьюмера Kafka, работающий в собственном выделенном потоке.
    /// <see cref="KafkaServiceCollectionExtensions.AddKafkaConsumer{TKey, TValue, THandler}"/> регистрирует
    /// столько экземпляров этого воркера, сколько указано в <see cref="KafkaConsumerSettings.ConsumersCount"/> -
    /// все они принадлежат одной consumer group, поэтому Kafka сама распределит партиции топика между ними.
    /// Именно так, согласно официальной документации Confluent, следует масштабировать обработку сообщений
    /// (несколько инстансов консьюмера в одной группе, каждый в своём потоке/процессе).
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
            // Confluent.Kafka's Consume() blocks the calling thread, so it must run on a dedicated
            // long-running thread rather than a regular thread-pool task.
            return Task.Factory.StartNew(
                () => RunConsumeLoop(stoppingToken),
                stoppingToken,
                TaskCreationOptions.LongRunning,
                TaskScheduler.Default);
        }

        private void RunConsumeLoop(CancellationToken stoppingToken)
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = _settings.BootstrapServers,
                GroupId = _settings.ConsumerGroup,
                AutoOffsetReset = _settings.AutoOffsetReset,
                EnableAutoCommit = _settings.EnableAutoCommit,
            };

            using var consumer = new ConsumerBuilder<TKey, string>(config)
                .SetErrorHandler((_, error) => _logger.LogError(
                    "Kafka consumer #{WorkerId} error on topic {Topic}: {Reason} (IsFatal: {IsFatal})",
                    _workerId, _settings.Topic, error.Reason, error.IsFatal))
                .SetPartitionsAssignedHandler((_, partitions) => _logger.LogInformation(
                    "Kafka consumer #{WorkerId} ({Group}/{Topic}) assigned partitions: [{Partitions}]",
                    _workerId, _settings.ConsumerGroup, _settings.Topic, string.Join(", ", partitions)))
                .SetPartitionsRevokedHandler((_, partitions) => _logger.LogInformation(
                    "Kafka consumer #{WorkerId} ({Group}/{Topic}) revoked partitions: [{Partitions}]",
                    _workerId, _settings.ConsumerGroup, _settings.Topic, string.Join(", ", partitions)))
                .Build();

            consumer.Subscribe(_settings.Topic);

            _logger.LogInformation(
                "Kafka consumer #{WorkerId} started for topic {Topic}, group {Group}",
                _workerId, _settings.Topic, _settings.ConsumerGroup);

            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    try
                    {
                        var consumeResult = consumer.Consume(stoppingToken);
                        if (consumeResult is null || consumeResult.IsPartitionEOF)
                        {
                            continue;
                        }

                        HandleMessage(consumeResult, stoppingToken).GetAwaiter().GetResult();

                        if (!_settings.EnableAutoCommit)
                        {
                            consumer.Commit(consumeResult);
                        }
                    }
                    catch (OperationCanceledException)
                    {
                        break;
                    }
                    catch (ConsumeException ex)
                    {
                        _logger.LogError(
                            ex,
                            "Kafka consumer #{WorkerId} failed to consume a message from {Topic}",
                            _workerId, _settings.Topic);
                    }
                    catch (Exception ex)
                    {
                        // A single failing message must not bring the whole worker (or the app) down.
                        _logger.LogError(
                            ex,
                            "Kafka consumer #{WorkerId} failed to process a message from {Topic}",
                            _workerId, _settings.Topic);
                    }
                }
            }
            finally
            {
                consumer.Close();
                _logger.LogInformation(
                    "Kafka consumer #{WorkerId} stopped for topic {Topic}, group {Group}",
                    _workerId, _settings.Topic, _settings.ConsumerGroup);
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
    }
}
