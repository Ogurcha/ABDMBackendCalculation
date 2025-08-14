using Abdm.Integration.MessageBroker.Kafka.Producer;

namespace Abdm.Integration.MessageBroker.Kafka.Serialization.Extensions;

public static partial class KafkaProducerBuilderExtensions
{
    public static KafkaProducerBuilder<TKey, TMessage> UseJsonKeySerializer<TKey, TMessage>(
        this KafkaProducerBuilder<TKey, TMessage> builder)
        where TKey : class
    {
        builder.KeySerializerFactory = _ => new JsonSerializer<TKey>();

        return builder;
    }

    public static KafkaProducerBuilder<TKey, TMessage> UseJsonMessageSerializer<TKey, TMessage>(
        this KafkaProducerBuilder<TKey, TMessage> builder)
        where TMessage : class
    {
        builder.MessageSerializerFactory = _ => new JsonSerializer<TMessage>();

        return builder;
    }
}
