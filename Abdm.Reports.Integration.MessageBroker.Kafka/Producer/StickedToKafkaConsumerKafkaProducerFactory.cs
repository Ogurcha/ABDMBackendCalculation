using System.Collections.Concurrent;
using Abdm.Integration.MessageBroker.Kafka.Consumer;

namespace Abdm.Integration.MessageBroker.Kafka.Producer;

internal class StickedToKafkaConsumerKafkaProducerFactory : KafkaProducerFactoryBase, IKafkaProducerFactory
{
    private readonly IConsumerContext _consumerContext;
    private readonly ProducersStorage _producersStorage;

    public StickedToKafkaConsumerKafkaProducerFactory(IConsumerContext consumerContext,
        IServiceProvider serviceProvider,
        IKafkaProducerOptionsResolver producerOptionsResolver,
        ProducersStorage producersStorage)
        : base(serviceProvider, producerOptionsResolver)
    {
        _consumerContext = consumerContext;
        _producersStorage = producersStorage;
    }

    public IKafkaProducer<TKey, TValue> Create<TKey, TValue>(string producerName)
    {
        var producerKey = new ProducerKey
        {
            ConsumerName = _consumerContext.Name,
            ProducerName = producerName.ToLowerInvariant()
        };

        // ReSharper disable once InconsistentlySynchronizedField
        if (_producersStorage.Value.TryGetValue(producerKey, out var producer))
        {
            return (IKafkaProducer<TKey, TValue>)producer;
        }

        lock (_producersStorage.Value)
        {
            if (_producersStorage.Value.TryGetValue(producerKey, out producer))
            {
                return (IKafkaProducer<TKey, TValue>)producer;
            }

            producer = CreateProducer<TKey, TValue>(producerName);
            _producersStorage.Value.TryAdd(producerKey, producer);

            return (IKafkaProducer<TKey, TValue>)producer;
        }
    }

    internal class ProducersStorage : IDisposable
    {
        public ConcurrentDictionary<ProducerKey, object> Value { get; } = new();

        public void Dispose()
        {
            foreach (var producer in Value.Values)
            {
                ((IKafkaProducer) producer).Dispose();
            }
        }
    }

    internal readonly struct ProducerKey : IEquatable<ProducerKey>
    {
        public bool Equals(ProducerKey other)
        {
            return ConsumerName == other.ConsumerName && ProducerName == other.ProducerName;
        }

        public override bool Equals(object? obj)
        {
            return obj is ProducerKey other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(ConsumerName, ProducerName);
        }

        public string ConsumerName { get; init; }
        public string ProducerName { get; init; }
    }
}
