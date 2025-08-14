namespace Abdm.Integration.MessageBroker.Kafka.Consumer;

public interface IKafkaMessageHandler<TKey, TMessage>
{
    Task Handle(TMessage message, MessageContext<TKey, TMessage> context);
}