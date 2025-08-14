using Confluent.Kafka;

namespace Abdm.Integration.MessageBroker.Kafka.Consumer;

public interface IBatchConsumerMessageHandler<TKey, TMessage>
{
    Task Handle(IReadOnlyCollection<ConsumeResult<TKey, TMessage>> consumeResults);
}
