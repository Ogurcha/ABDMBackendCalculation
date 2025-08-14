namespace Abdm.Integration.MessageBroker.Kafka.Producer;

public record ProduceResult(
    string Topic,
    int Partition,
    long Offset);
