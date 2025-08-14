using Abdm.Integration.MessageBroker.Kafka.Consumer;

namespace Abdm.Integration.MessageBroker.Kafka.Serialization.Extensions;

public static partial class KafkaConsumerBuilderExtensions
{
    public static KafkaConsumerBuilder<TKey, TMessage> UseJsonKeyDeserializer<TKey, TMessage>(
        this KafkaConsumerBuilder<TKey, TMessage> builder)
        where TKey : class
    {
        builder.KeyDeserializerFactory = _ => new JsonSerializer<TKey>();

        return builder;
    }

    public static KafkaConsumerBuilder<TKey, TMessage> UseJsonMessageDeserializer<TKey, TMessage>(
        this KafkaConsumerBuilder<TKey, TMessage> builder)
        where TMessage : class
    {
        builder.MessageDeserializerFactory = _ => new JsonSerializer<TMessage>();

        return builder;
    }
}
