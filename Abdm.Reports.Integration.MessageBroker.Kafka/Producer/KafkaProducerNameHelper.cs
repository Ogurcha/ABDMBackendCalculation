namespace Abdm.Integration.MessageBroker.Kafka.Producer;

internal static class KafkaProducerNameHelper
{
    public static string GetDefaultName<TMessage>()
        => typeof(TMessage).Name;
}
