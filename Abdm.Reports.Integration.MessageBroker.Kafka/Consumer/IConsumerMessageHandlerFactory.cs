namespace Abdm.Integration.MessageBroker.Kafka.Consumer;

public interface IConsumerMessageHandlerFactory<TKey, TMessage>
{
    IConsumerMessageHandler<TKey, TMessage> Create(HandleMessage<TKey, TMessage> handleMessage);

    IBatchConsumerMessageHandler<TKey, TMessage> CreateBatch(HandleMessage<TKey, TMessage> handleMessage);
}
