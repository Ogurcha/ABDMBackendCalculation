namespace Abdm.Integration.MessageBroker.Kafka.Consumer;

public interface IKafkaConsumer
{
    Task Start();

    Task Stop();
}