using Confluent.Kafka;

namespace Abdm.Integration.MessageBroker.Kafka.Consumer;

public interface IConsumerMessageHandler<TKey, TMessage>
{
    Task Handle(ConsumeResult<TKey, TMessage> consumeResult);
}
