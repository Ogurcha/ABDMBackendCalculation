using Confluent.Kafka;
using Microsoft.Extensions.Logging;

namespace Abdm.Integration.MessageBroker.Kafka.Consumer;

internal abstract class KafkaConsumerBase<TKey, TMessage> : IKafkaConsumer
{
    protected readonly string _consumerName;
    protected readonly string _consumerBaseName;
    protected readonly KafkaConsumerOptions _options;
    protected readonly ILogger<KafkaConsumerBase<TKey, TMessage>> _logger;
    private readonly IKafkaUnparsedMessageHandler _unparsedMessageHandler;
    private readonly IDeserializer<TKey>? _keyDeserializer;
    private readonly IDeserializer<TMessage>? _messageDeserializer;
    private readonly CancellationTokenSource _cts;
    private readonly TaskCompletionSource _consumingStoppedTcs;

    protected KafkaConsumerBase(
        string consumerName,
        string consumerBaseName,
        KafkaConsumerOptions options,
        ILogger<KafkaConsumerBase<TKey, TMessage>> logger,
        IDeserializer<TKey>? keyDeserializer,
        IDeserializer<TMessage>? messageDeserializer,
        IKafkaUnparsedMessageHandler unparsedMessageHandler)
    {
        _consumerName = consumerName;
        _consumerBaseName = consumerBaseName;
        _options = options;
        _logger = logger;
        _keyDeserializer = keyDeserializer;
        _messageDeserializer = messageDeserializer;
        _unparsedMessageHandler = unparsedMessageHandler;
        _cts = new CancellationTokenSource();
        _consumingStoppedTcs = new TaskCompletionSource();
    }

    public async Task Start()
    {
        await Task.Yield();

        var cancellationToken = _cts.Token;

        while (!cancellationToken.IsCancellationRequested)
        {
            IConsumer<TKey, TMessage>? consumer = null;

            try
            {
                consumer = BuildConsumer(consumerBaseName: _consumerBaseName,
                    consumerName: _consumerName,
                    options: _options,
                    keyDeserializer: _keyDeserializer,
                    messageDeserializer: _messageDeserializer,
                    logger: _logger);

                consumer.Subscribe(_options.Topic);
                _logger.LogInformation("Kafka consumer {ConsumerName} subscribed to {Topic}",
                    _consumerName, _options.Topic);

                await Consume(consumer, cancellationToken);
            }
            catch (ConsumeException e)
            {
                _logger.LogError(e,
                    "ConsumeException occured while consuming {Topic} ({@Error}, {ConsumerName})",
                    _options.Topic, e.Error, _consumerName);
            }
            catch (TaskCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                _logger.LogInformation("Consuming {Topic} cancelled ({ConsumerName})",
                    _options.Topic, _consumerName);
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("Consuming {Topic} cancelled ({ConsumerName})",
                    _options.Topic, _consumerName);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Exception while consuming {Topic} occured ({ConsumerName})",
                    _options.Topic, _consumerName);
            }
            finally
            {
                consumer?.Close();
                consumer?.Dispose();
            }

            if (cancellationToken.IsCancellationRequested)
            {
                break;
            }

            await Task.Delay(_options.WaitForReconnect, cancellationToken);
            _logger.LogInformation("Kafka consumer {ConsumerName} reconnecting to {Topic}...",
                _consumerName, _options.Topic);
        }

        _logger.LogInformation("Kafka consumer {ConsumerName} stopped",
            _consumerName);

        _consumingStoppedTcs.SetResult();
    }

    protected abstract Task Consume(IConsumer<TKey, TMessage> consumer, CancellationToken cancellationToken);

    public async Task Stop()
    {
        _logger.LogInformation("Kafka consumer {ConsumerName} stopping requested", _consumerName);

        _cts.Cancel();

        await _consumingStoppedTcs.Task;

        _logger.LogInformation("Kafka consumer {ConsumerName} stopped", _consumerName);
    }

    protected async Task KeyDeserializationFailed(ConsumeException e)
    {
        _logger.LogWarning(e,
            "Exception while deserializing message key at {Offset} in {Partition} partition of {Topic} topic by {ConsumerName}",
            e.ConsumerRecord.Offset.Value, e.ConsumerRecord.Partition.Value, e.ConsumerRecord.Topic,
            _consumerName);

        await _unparsedMessageHandler.KeyDeserializationFailed(consumeResult: e.ConsumerRecord,
            targetType: typeof(TKey),
            exception: e);
    }

    protected async Task ValueDeserializationFailed(ConsumeException e)
    {
        _logger.LogWarning(e,
            "Exception while deserializing message value at {Offset} in {Partition} partition of {Topic} topic by {ConsumerName}",
            e.ConsumerRecord.Offset.Value, e.ConsumerRecord.Partition.Value, e.ConsumerRecord.Topic,
            _consumerName);

        await _unparsedMessageHandler.ValueDeserializationFailed(consumeResult: e.ConsumerRecord,
            targetType: typeof(TMessage),
            exception: e);
    }

    protected void CommitOffset(
        IConsumer<TKey, TMessage> consumer, ConsumeResult<byte[], byte[]> consumeResult)
    {
        CommitOffset(consumer, new ConsumeResult<TKey, TMessage>
        {
            Offset = consumeResult.Offset,
            Partition = consumeResult.Partition,
            Topic = consumeResult.Topic,
            TopicPartitionOffset = consumeResult.TopicPartitionOffset,
            Message = new Message<TKey, TMessage>(),
            LeaderEpoch = consumeResult.LeaderEpoch
        });
    }

    protected void CommitOffset(
        IConsumer<TKey, TMessage> consumer, ConsumeResult<TKey, TMessage> consumeResult)
    {
        try
        {
            consumer.Commit(consumeResult);

            _logger.LogDebug(
                "Offset committed {Offset} in {Partition} partition of {Topic} topic by {ConsumerName}",
                consumeResult.Offset.Value, consumeResult.Partition.Value,
                consumeResult.Topic, _consumerName);
        }
        catch (KafkaException e) when (e.Error.Code == ErrorCode.IllegalGeneration)
        {
            _logger.LogError(e,
                "Invalid group generation id while commiting offsets ({Offset}, {Partition}, {Topic}, {ConsumerName})",
                consumeResult.Offset.Value, consumeResult.Partition.Value,
                consumeResult.Topic, _consumerName);
        }
        catch (Exception e)
        {
            _logger.LogError(e,
                "Exception while commiting {Offset} in {Partition} partition of {Topic} topic by {ConsumerName}",
                consumeResult.Offset.Value, consumeResult.Partition.Value,
                consumeResult.Topic, _consumerName);

            throw;
        }
    }

    protected void CommitOffsets(
        IConsumer<TKey, TMessage> consumer, IReadOnlyCollection<TopicPartitionOffset> offsets)
    {
        try
        {
            consumer.Commit(offsets);

            _logger.LogDebug("Offsets committed ({@Offsets}, {ConsumerName})", offsets, _consumerName);
        }
        catch (KafkaException e) when (e.Error.Code == ErrorCode.IllegalGeneration)
        {
            _logger.LogError(e,
                "Invalid group generation id while commiting offsets ({@Offsets}, {ConsumerName})", offsets,
                _consumerName);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Exception while commiting offsets ({@Offsets}, {ConsumerName})", offsets,
                _consumerName);

            throw;
        }
    }

    private IConsumer<TKey, TMessage> BuildConsumer(
        string consumerBaseName,
        string consumerName,
        KafkaConsumerOptions options,
        IDeserializer<TKey>? keyDeserializer,
        IDeserializer<TMessage>? messageDeserializer,
        ILogger logger)
    {
        var consumerConfig = new ConsumerConfig
        {
            GroupId = options.ConsumerGroup,
            BootstrapServers = options.BootstrapServers,
            ClientId = consumerName,
            AutoOffsetReset = options.AutoOffsetReset,
            EnableAutoCommit = options.EnableAutoCommit,
            SessionTimeoutMs = options.SessionTimeoutMs,
            MaxPollIntervalMs = options.MaxPollIntervalMs,
            PartitionAssignmentStrategy = options.RebalancingStrategy
        };

        if (options.SecurityProtocol is {} securityProtocol)
        {
            consumerConfig.SecurityProtocol = securityProtocol;
        }

        if (options.Sasl is {} sasl)
        {
            if (sasl.SaslMechanism is {} saslMechanism)
            {
                consumerConfig.SaslMechanism = saslMechanism;
            }

            if (sasl.SaslOAuthBearerTokenEndpointUrl is {} saslOAuthBearerTokenEndpointUrl)
            {
                consumerConfig.SaslOauthbearerTokenEndpointUrl = saslOAuthBearerTokenEndpointUrl;
            }

            if (sasl.SaslOAuthBearerClientId is {} saslOAuthBearerClientId)
            {
                consumerConfig.SaslOauthbearerClientId = saslOAuthBearerClientId;
            }

            if (sasl.SaslOAuthBearerClientSecret is {} saslOAuthBearerClientSecret)
            {
                consumerConfig.SaslOauthbearerClientSecret = saslOAuthBearerClientSecret;
            }

            if (sasl.SaslOAuthBearerMethod is {} saslOAuthBearerMethod)
            {
                consumerConfig.SaslOauthbearerMethod = saslOAuthBearerMethod;
            }
        }

        if (options.EnableSslCertificateVerification is {} enableSslCertificateVerification)
        {
            consumerConfig.EnableSslCertificateVerification = enableSslCertificateVerification;
        }

        if (options.QueuedMinMessages is not null)
        {
            consumerConfig.QueuedMinMessages = options.QueuedMinMessages;
        }

        if (options.QueuedMaxMessagesKbytes is not null)
        {
            consumerConfig.QueuedMaxMessagesKbytes = options.QueuedMaxMessagesKbytes;
        }

        var consumerBuilder = new ConsumerBuilder<TKey, TMessage>(consumerConfig);

        if (keyDeserializer is not null)
        {
            consumerBuilder.SetKeyDeserializer(keyDeserializer);
        }

        if (messageDeserializer is not null)
        {
            consumerBuilder.SetValueDeserializer(messageDeserializer);
        }

        consumerBuilder.SetErrorHandler((_, error) =>
        {
            logger.LogError("Kafka consumer {ConsumerName} error {@Error}", consumerName, error);
        });

        consumerBuilder.SetLogHandler((_, message) =>
        {
            var level = message.Level switch
            {
                SyslogLevel.Emergency => LogLevel.Error,
                SyslogLevel.Alert => LogLevel.Error,
                SyslogLevel.Critical => LogLevel.Error,
                SyslogLevel.Error => LogLevel.Error,
                SyslogLevel.Warning => LogLevel.Warning,
                SyslogLevel.Notice => LogLevel.Information,
                SyslogLevel.Info => LogLevel.Debug,
                SyslogLevel.Debug => LogLevel.Trace,
                _ => LogLevel.Debug
            };

            logger.Log(level, "Kafka consumer {ConsumerName} log {@Message}", consumerName, message);
        });

        consumerBuilder.SetPartitionsAssignedHandler((_, list) =>
        {
            logger.LogInformation(
                "Topic {Topic} partitions {@TopicPartitions} assigned to {ConsumerName} consumer",
                options.Topic, list, consumerName);
        });

        consumerBuilder.SetPartitionsLostHandler((_, list) =>
        {
            logger.LogInformation(
                "Topic {Topic} partitions {@TopicPartitions} lost by {ConsumerName} consumer",
                options.Topic, list, consumerName);
        });

        consumerBuilder.SetPartitionsRevokedHandler((_, list) =>
        {
            logger.LogInformation(
                "Topic {Topic} partitions {@TopicPartitions} revoked from {ConsumerName} consumer",
                options.Topic, list, consumerName);
        });

        return consumerBuilder.Build();
    }
}
