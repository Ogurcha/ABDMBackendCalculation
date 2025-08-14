using Microsoft.Extensions.DependencyInjection;

namespace Abdm.Integration.MessageBroker.Kafka.Producer.Extensions;

public static class KafkaProducerConfigurationBuilderExtensions
{
    public static KafkaProducerBuilder<TKey, TMessage> LoadFromConfiguration<TKey, TMessage>(
        this KafkaProducerConfigurationBuilder<TKey, TMessage> builder, string configurationSection)
    {
        builder.OptionsBuilderActions.Add(optionsBuilder =>
        {
            optionsBuilder.BindConfiguration(configurationSection);
        });

        return builder.ProducerBuilder;
    }

    public static KafkaProducerBuilder<TKey, TMessage> Configure<TKey, TMessage>(
        this KafkaProducerConfigurationBuilder<TKey, TMessage> configurationBuilder,
        Action<KafkaProducerOptions> configure)
    {
        configurationBuilder.OptionsBuilderActions.Add(optionsBuilder =>
        {
            optionsBuilder.Configure(configure);
        });

        return configurationBuilder.ProducerBuilder;
    }
}
