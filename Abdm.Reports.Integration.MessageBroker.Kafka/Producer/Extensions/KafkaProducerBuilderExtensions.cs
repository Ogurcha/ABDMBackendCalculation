using Microsoft.Extensions.DependencyInjection;

namespace Abdm.Integration.MessageBroker.Kafka.Producer.Extensions;

public static partial class KafkaProducerBuilderExtensions
{
    public static KafkaProducerBuilder UseStickedToKafkaConsumerProducerFactory<TKey, TMessage>(
        this KafkaProducerBuilder builder)
    {
        builder.Services.TryAddStickedToKafkaConsumerKafkaProducerFactory();

        builder.KafkaProducerFactoryFactory = sp =>
            sp.GetRequiredService<StickedToKafkaConsumerKafkaProducerFactory>();

        return builder;
    }
}
