using Confluent.Kafka;

namespace Abdm.Integration.MessageBroker.Kafka.Consumer;

public interface IBatchConsumerLimiter<TKey, TValue>
{
    bool NeedToCompleteBatch(IReadOnlyList<ConsumeResult<TKey, TValue>> consumeResults);
}
