namespace Abdm.Integration.MessageBroker.Kafka.Consumer;

internal class KafkaConsumerRegistry
{
    public HashSet<string> RegisteredConsumerBaseNames { get; } = new();
}