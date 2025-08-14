using Confluent.Kafka;

namespace Abdm.Integration.MessageBroker.Kafka.Consumer;

public class KafkaConsumerSaslOptions
{
    public SaslMechanism? SaslMechanism { get; set; }

    public string? SaslOAuthBearerTokenEndpointUrl { get; set; }

    public string? SaslOAuthBearerClientId { get; set; }

    public string? SaslOAuthBearerClientSecret { get; set; }

    public SaslOauthbearerMethod? SaslOAuthBearerMethod { get; set; }
}