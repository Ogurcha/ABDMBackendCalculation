using System.Diagnostics;
using System.Text;
using Confluent.Kafka;
using Microsoft.Extensions.Logging;

namespace Abdm.Integration.MessageBroker.Kafka.Producer;

internal class KafkaProducer<TKey, TMessage> : IKafkaProducer<TKey, TMessage>
{
    private readonly KafkaProducerOptions _options;
    private readonly ILogger<KafkaProducer<TKey, TMessage>> _logger;
    private readonly IProducer<TKey, TMessage> _producer;

    public KafkaProducer(string producerName,
        KafkaProducerOptions options,
        ILogger<KafkaProducer<TKey, TMessage>> logger,
        ISerializer<TKey>? keySerializer,
        ISerializer<TMessage>? messageSerializer)
    {
        Name = producerName;
        _options = options;
        _logger = logger;
        _producer = BuildProducer(producerName, options, logger,
            keySerializer, messageSerializer);
    }

    public string Name { get; }

    public Task<ProduceResult> Produce(TKey key, TMessage message,
        IReadOnlyDictionary<string, string>? headers = null)
    {
        return Produce(key, message, Partition.Any, headers);
    }

    public async Task<ProduceResult> Produce(TKey key, TMessage message,
        int partitionNumber, IReadOnlyDictionary<string, string>? headers = null)
    {
        _logger.LogTrace("<Produce>: {Key}", key);

        try
        {
            var kafkaMessage = new Message<TKey, TMessage>
            {
                Key = key,
                Value = message,
                Headers = BuildHeaders(headers),
                Timestamp = Timestamp.Default
            };

            var topicPartition = new TopicPartition(_options.Topic, partitionNumber);
            var deliveryResult = await _producer.ProduceAsync(topicPartition, kafkaMessage);

            _logger.LogInformation("Kafka message produced to {Topic} topic at {Offset} offset in {Partition} partition with {@Key} key",
                deliveryResult.Topic, deliveryResult.Offset.Value, deliveryResult.Partition.Value, key);

            return new ProduceResult(
                Topic: deliveryResult.Topic,
                Partition: deliveryResult.Partition.Value,
                Offset: deliveryResult.Offset.Value);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Exception while producing kafka message to {Topic} topic with {@Key} key",
                _options.Topic, key);

            throw;
        }
    }

    public async Task<IReadOnlyList<DeliveryReport<TKey, TMessage>>> BatchProduce(
        IEnumerable<BatchProduceMessage<TKey, TMessage>> messages,
        CancellationToken cancellationToken = default)
    {
        _logger.LogTrace("<BatchProduce>");

        var sw = Stopwatch.StartNew();
        var kafkaMessages = new List<Message<TKey, TMessage>>();

        foreach (var (key, value, headers) in messages)
        {
            if (key == null)
            {
                continue;
            }
            
            var message = new Message<TKey, TMessage>
            {
                Key = key,
                Value = value,
                Headers = BuildHeaders(
                    headers: headers),
                Timestamp = Timestamp.Default
            };
            kafkaMessages.Add(message);
        }

        _logger.LogInformation("Built {TotalMessagesCount} kafka messages to send in batch", kafkaMessages.Count);

        if (kafkaMessages.Count == 0)
        {
            return Array.Empty<DeliveryReport<TKey, TMessage>>();
        }

        var tcs = new TaskCompletionSource<IReadOnlyList<DeliveryReport<TKey, TMessage>>>();
        var deliveryReports = new List<DeliveryReport<TKey, TMessage>>(kafkaMessages.Count);

        var messagesToProduce = kafkaMessages;
        foreach (Message<TKey, TMessage> message in messagesToProduce)
        {
            _producer.Produce(_options.Topic, message, dr => DeliveryHandler(message, dr));
        }

        void DeliveryHandler(Message<TKey, TMessage>? message, DeliveryReport<TKey, TMessage> deliveryReport)
        {
            lock (deliveryReports)
            {
                if (message == null)
                {
                    return;
                }

                if (kafkaMessages.Contains(message))
                {
                    kafkaMessages.Remove(message);
                }
                else
                {
                    tcs.SetException(new Exception("Invalid delivery report message"));
                    throw new InvalidOperationException("Invalid delivery report message");
                }

                deliveryReports.Add(deliveryReport);

                if (deliveryReport.Error.IsError)
                {
                    _logger.LogWarning("Kafka message sending failed ({@Error}, {MessageKey})",
                        deliveryReport.Error, message.Key);
                }
                else
                {
                    _logger.LogDebug("Kafka message produced to {Topic} topic at {Offset} offset in {Partition} partition with {@Key} key",
                        deliveryReport.Topic, deliveryReport.Offset.Value, deliveryReport.Partition.Value, message.Key);
                }
                
                if (kafkaMessages.Count == 0)
                {
                    _logger.LogInformation("Kafka batch produce finished in {ElapsedMilliseconds}ms ({TotalMessagesCount} messages)",
                        sw.ElapsedMilliseconds, deliveryReports.Count);
                    tcs.SetResult(deliveryReports);
                }
            }
        }

        return await tcs.Task.WaitAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<DeliveryReport<TKey, TMessage>>> BatchProduce(
        IEnumerable<KeyValuePair<TKey, TMessage>> messages,
        CancellationToken cancellationToken = default)
    {
        return await BatchProduce(
            messages: messages
                .Select(m => new BatchProduceMessage<TKey, TMessage>(
                    Key: m.Key,
                    Value: m.Value,
                    Headers: null)),
            cancellationToken: cancellationToken);
    }

    private static IProducer<TKey, TMessage> BuildProducer(
        string producerName,
        KafkaProducerOptions options, 
        ILogger logger,
        ISerializer<TKey>? keySerializer,
        ISerializer<TMessage>? messageSerializer)
    {
        var producerConfig = new ProducerConfig
        {
            BootstrapServers = options.BootstrapServers,
            Acks = options.Acks,
            ClientId = producerName,
            MessageTimeoutMs = (int) options.MessageTimeout.TotalMilliseconds,
            MessageMaxBytes = options.MessageMaxBytes
        };

        if (options.Linger is not null)
        {
            producerConfig.LingerMs = options.Linger.Value.TotalMilliseconds;
        }

        var producerBuilder = new ProducerBuilder<TKey, TMessage>(producerConfig);

        if (keySerializer is not null)
        {
            producerBuilder.SetKeySerializer(keySerializer);
        }

        if (messageSerializer is not null)
        {
            producerBuilder.SetValueSerializer(messageSerializer);
        }

        producerBuilder.SetErrorHandler((_, error) =>
        {
            logger.LogError("Kafka {ProducerName} producer error {@Error}",
                producerName, error);
        });

        producerBuilder.SetLogHandler((_, message) =>
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

            logger.Log(level, "Kafka {ProducerName} producer log {@Message}",
                producerName, message);
        });

        logger.LogInformation("Creating producer {ProducerName} with {@Options} options",
            producerName, options);

        return producerBuilder.Build();
    }

    private static Headers BuildHeaders(IReadOnlyDictionary<string, string>? headers)
    {
        var result = new Headers();

        if (headers is not null)
        {
            foreach (var (key, value) in headers)
            {
                result.Add(key, Encoding.UTF8.GetBytes(value));
            }
        }

        return result;
    }

    public void Dispose()
    {
        try
        {
            _producer.Dispose();
            _logger.LogInformation("Kafka producer {ProducerName} disposed", Name);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Exception while disposing kafka producer {ProducerName}", Name);
        }
    }
}
