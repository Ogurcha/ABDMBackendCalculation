using Confluent.Kafka;

namespace Abdm.Integration.MessageBroker.Kafka.Consumer;

internal class DefaultBatchConsumerLimiter<TKey, TValue> : IBatchConsumerLimiter<TKey, TValue>
{
    public bool NeedToCompleteBatch(IReadOnlyList<ConsumeResult<TKey, TValue>> consumeResults)
    {
        return false;
    }
}
