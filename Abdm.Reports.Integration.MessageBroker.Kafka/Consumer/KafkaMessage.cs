namespace Abdm.Integration.MessageBroker.Kafka.Consumer;

public record KafkaMessage<TKey, TValue>(TKey Key, TValue Value);