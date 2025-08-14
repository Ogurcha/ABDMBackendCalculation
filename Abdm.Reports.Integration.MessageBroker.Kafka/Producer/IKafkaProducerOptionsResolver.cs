namespace Abdm.Integration.MessageBroker.Kafka.Producer;

public interface IKafkaProducerOptionsResolver
{
    KafkaProducerOptions GetOptions(string producerName);
}
