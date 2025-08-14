using Microsoft.Extensions.DependencyInjection;

namespace Abdm.Integration.MessageBroker.Kafka.Consumer.Extensions;

public static class KafkaConsumerConfigurationBuilderExtensions
{
    public static KafkaConsumerBuilder<TKey, TMessage> LoadFromConfiguration<TKey, TMessage>(
        this KafkaConsumerConfigurationBuilder<TKey, TMessage> configurationBuilder,
        string configurationSection)
    {
        configurationBuilder.OptionsBuilderActions.Add(optionsBuilder =>
        {
            optionsBuilder.BindConfiguration(configurationSection);
        });

        return configurationBuilder.ConsumerBuilder;
    }

    public static KafkaConsumerBuilder<TKey, TMessage> Configure<TKey, TMessage>(
        this KafkaConsumerConfigurationBuilder<TKey, TMessage> configurationBuilder,
        Action<KafkaConsumerOptions> configure)
    {
        configurationBuilder.OptionsBuilderActions.Add(optionsBuilder =>
        {
            optionsBuilder.Configure(configure);
        });

        return configurationBuilder.ConsumerBuilder;
    }
}
