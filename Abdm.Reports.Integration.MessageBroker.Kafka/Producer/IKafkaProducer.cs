using Confluent.Kafka;

namespace Abdm.Integration.MessageBroker.Kafka.Producer;

public interface IKafkaProducer
{
    string Name { get; }

    internal void Dispose();
}

public interface IKafkaProducer<TKey, TMessage>: IKafkaProducer
{
    Task<ProduceResult> Produce(TKey key, TMessage message,
        IReadOnlyDictionary<string, string>? headers = null);

    Task<ProduceResult> Produce(TKey key, TMessage message,
        int partitionNumber,
        IReadOnlyDictionary<string, string>? headers = null);

    Task<IReadOnlyList<DeliveryReport<TKey, TMessage>>> BatchProduce(
        IEnumerable<KeyValuePair<TKey, TMessage>> messages,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<DeliveryReport<TKey, TMessage>>> BatchProduce(
        IEnumerable<BatchProduceMessage<TKey, TMessage>> messages,
        CancellationToken cancellationToken = default);
}
