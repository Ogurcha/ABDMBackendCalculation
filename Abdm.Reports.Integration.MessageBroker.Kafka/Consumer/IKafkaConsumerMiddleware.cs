namespace Abdm.Integration.MessageBroker.Kafka.Consumer;

public interface IKafkaConsumerMiddleware<TKey, TMessage>
{
    Task Invoke(MessageContext<TKey, TMessage> context,
        HandleMessage<TKey, TMessage> next);
}