namespace Abdm.Integration.MessageBroker.Kafka.Consumer.Exceptions;

public class InvalidKafkaConsumerConfigurationException : Exception
{
    public InvalidKafkaConsumerConfigurationException(string consumerBaseName, string error)
        : base($"Kafka consumer '{consumerBaseName}' has invalid configuration ({error})")
    {
    }
}
