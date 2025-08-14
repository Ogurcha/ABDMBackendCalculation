namespace Abdm.Integration.MessageBroker.Kafka.Consumer;

public interface IKafkaConsumerOptionsResolver
{
    KafkaConsumerOptions ResolveConsumerOptions(string consumerName);
}