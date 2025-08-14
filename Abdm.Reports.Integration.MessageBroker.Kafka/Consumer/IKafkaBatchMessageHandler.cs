namespace Abdm.Integration.MessageBroker.Kafka.Consumer;

public interface IKafkaBatchMessageHandler<TKey, TValue>
{
    Task Handle(IReadOnlyCollection<KafkaMessage<TKey, TValue>> messages);
}
