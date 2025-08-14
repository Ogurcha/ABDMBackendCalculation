using Confluent.Kafka;

namespace Abdm.Integration.MessageBroker.Kafka.Consumer;

public interface IKafkaUnparsedMessageHandler
{
    Task KeyDeserializationFailed(ConsumeResult<byte[], byte[]> consumeResult,
        Type targetType, Exception exception);

    Task ValueDeserializationFailed(ConsumeResult<byte[], byte[]> consumeResult,
        Type targetType, Exception exception);
}