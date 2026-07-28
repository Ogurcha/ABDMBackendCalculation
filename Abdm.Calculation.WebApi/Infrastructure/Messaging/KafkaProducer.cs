using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using Microsoft.Extensions.Logging;

namespace Abdm.Calculation.WebApi.Infrastructure.Messaging
{
    /// <summary>
    /// Продюсер Kafka поверх официального клиента Confluent.Kafka.
    /// Значение сериализуется в JSON, ключ передаётся во встроенном (де)сериализаторе Confluent.Kafka,
    /// поэтому TKey должен быть одним из типов, поддерживаемых им "из коробки" (string, Null, Ignore и т.п.).
    /// Регистрируется как singleton и владеет одним переиспользуемым IProducer на весь процесс,
    /// что соответствует рекомендациям официальной документации.
    /// </summary>
    public sealed class KafkaProducer<TKey, TValue> : IKafkaProducer<TKey, TValue>, IDisposable
    {
        private readonly IProducer<TKey, string> _producer;
        private readonly string _topic;
        private readonly ILogger<KafkaProducer<TKey, TValue>> _logger;

        public KafkaProducer(KafkaProducerSettings settings, ILogger<KafkaProducer<TKey, TValue>> logger)
        {
            _topic = settings.Topic;
            _logger = logger;

            var config = new ProducerConfig
            {
                BootstrapServers = settings.BootstrapServers,
                Acks = Acks.All,
                EnableIdempotence = true,
            };

            _producer = new ProducerBuilder<TKey, string>(config)
                .SetErrorHandler((_, error) => _logger.LogError(
                    "Kafka producer error for topic {Topic}: {Reason} (IsFatal: {IsFatal})",
                    _topic, error.Reason, error.IsFatal))
                .Build();
        }

        public async Task Produce(TKey key, TValue value, CancellationToken cancellationToken = default)
        {
            var payload = JsonSerializer.Serialize(value);
            var message = new Message<TKey, string> { Key = key, Value = payload };

            var deliveryResult = await _producer.ProduceAsync(_topic, message, cancellationToken).ConfigureAwait(false);

            _logger.LogDebug(
                "Produced message to {Topic}/{Partition}@{Offset}",
                deliveryResult.Topic, deliveryResult.Partition, deliveryResult.Offset);
        }

        public void Dispose()
        {
            _producer.Flush(TimeSpan.FromSeconds(10));
            _producer.Dispose();
        }
    }
}
