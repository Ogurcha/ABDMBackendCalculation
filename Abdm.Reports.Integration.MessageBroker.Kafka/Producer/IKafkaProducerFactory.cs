namespace Abdm.Integration.MessageBroker.Kafka.Producer;

public interface IKafkaProducerFactory
{
    IKafkaProducer<TKey, TMessage> Create<TKey, TMessage>(string producerName);
}
