using Confluent.Kafka;

namespace Abdm.Integration.MessageBroker.Kafka.Consumer;

internal class DefaultUnparsedMessageHandler : IKafkaUnparsedMessageHandler
{
    public Task KeyDeserializationFailed(ConsumeResult<byte[], byte[]> consumeResult,
        Type targetType, Exception exception)
    {
        return Task.FromException(exception);
    }

    public Task ValueDeserializationFailed(ConsumeResult<byte[], byte[]> consumeResult,
        Type targetType, Exception exception)
    {
        return Task.FromException(exception);
    }
}
