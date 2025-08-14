namespace Abdm.Integration.MessageBroker.Kafka.Producer.Exceptions;

public class InvalidKafkaProducerConfigurationException : Exception
{
    public InvalidKafkaProducerConfigurationException(string producerName, string error)
        : base($"Kafka producer '{producerName}' has invalid configuration ({error})")
    {
    }
}
