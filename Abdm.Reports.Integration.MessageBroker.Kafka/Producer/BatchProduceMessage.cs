namespace Abdm.Integration.MessageBroker.Kafka.Producer;

public readonly record struct BatchProduceMessage<TKey, TValue>(
    TKey Key,
    TValue Value,
    IReadOnlyDictionary<string, string>? Headers);
