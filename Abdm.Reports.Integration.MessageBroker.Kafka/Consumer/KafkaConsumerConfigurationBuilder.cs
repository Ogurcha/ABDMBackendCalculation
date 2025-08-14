using System.ComponentModel;
using Microsoft.Extensions.Options;

namespace Abdm.Integration.MessageBroker.Kafka.Consumer;

public class KafkaConsumerConfigurationBuilder<TKey, TMessage>
{
    public KafkaConsumerConfigurationBuilder(KafkaConsumerBuilder<TKey, TMessage> consumerBuilder)
    {
        ConsumerBuilder = consumerBuilder;
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public List<Action<OptionsBuilder<KafkaConsumerOptions>>> OptionsBuilderActions { get; } = new();

    [EditorBrowsable(EditorBrowsableState.Never)]
    public KafkaConsumerBuilder<TKey, TMessage> ConsumerBuilder { get; }
}
