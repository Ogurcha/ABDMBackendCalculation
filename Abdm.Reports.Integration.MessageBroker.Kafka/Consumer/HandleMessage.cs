namespace Abdm.Integration.MessageBroker.Kafka.Consumer;

public delegate Task HandleMessage<TKey, TMessage>(
    MessageContext<TKey, TMessage> context);
