using System.ComponentModel;
using Microsoft.Extensions.Options;

namespace Abdm.Integration.MessageBroker.Kafka.Producer;

public class KafkaProducerConfigurationBuilder<TKey, TMessage>
{
    public KafkaProducerConfigurationBuilder(KafkaProducerBuilder<TKey, TMessage> producerBuilder)
    {
        ProducerBuilder = producerBuilder;
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public List<Action<OptionsBuilder<KafkaProducerOptions>>> OptionsBuilderActions { get; } = new();

    [EditorBrowsable(EditorBrowsableState.Never)]
    public KafkaProducerBuilder<TKey, TMessage> ProducerBuilder { get; }
}
