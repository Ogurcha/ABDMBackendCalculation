using System.Collections.Concurrent;

namespace Abdm.Integration.MessageBroker.Kafka.Producer;

internal class DefaultKafkaProducerFactory : KafkaProducerFactoryBase, IKafkaProducerFactory, IDisposable
{
    private readonly ConcurrentDictionary<string, object> _producers = new();

    public DefaultKafkaProducerFactory(IServiceProvider serviceProvider,
        IKafkaProducerOptionsResolver producerOptionsResolver)
        : base(serviceProvider, producerOptionsResolver)
    {
    }

    public IKafkaProducer<TKey, TMessage> Create<TKey, TMessage>(string producerName)
    {
        producerName = producerName.ToLowerInvariant();

        // ReSharper disable once InconsistentlySynchronizedField
        if (_producers.TryGetValue(producerName, out var producer))
        {
            return (IKafkaProducer<TKey, TMessage>) producer;
        }

        lock (_producers)
        {
            if (_producers.TryGetValue(producerName, out producer))
            {
                return (IKafkaProducer<TKey, TMessage>) producer;
            }

            producer = CreateProducer<TKey, TMessage>(producerName);
            _producers.TryAdd(producerName, producer);

            return (IKafkaProducer<TKey, TMessage>)producer;
        }
    }

    public void Dispose()
    {
        foreach (var producer in _producers.Values)
        {
            ((IKafkaProducer) producer).Dispose();
        }
    }
}
