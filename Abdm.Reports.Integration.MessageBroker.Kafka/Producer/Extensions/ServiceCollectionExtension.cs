using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Abdm.Integration.MessageBroker.Kafka.Producer.Extensions;

public static class ServiceCollection
{
    public static IServiceCollection AddKafkaProducer<TKey, TMessage>(
        this IServiceCollection services,
        string producerName,
        Action<KafkaProducerBuilder<TKey, TMessage>> builderAction)
    {
        var producerBuilder = new KafkaProducerBuilder<TKey, TMessage>(
            services: services,
            producerName: producerName);

        builderAction(producerBuilder);

        producerBuilder.RegisterProducer();

        return services;
    }

    public static IServiceCollection AddKafkaProducer<TKey, TMessage>(
        this IServiceCollection services,
        Action<KafkaProducerBuilder<TKey, TMessage>> builderAction)
    {
        var producerBuilder = new KafkaProducerBuilder<TKey, TMessage>(
            services: services,
            producerName: KafkaProducerNameHelper.GetDefaultName<TMessage>());

        builderAction(producerBuilder);

        producerBuilder.RegisterProducer();

        return services;
    }

    public static void TryAddKafkaProducerOptionsResolver(this IServiceCollection services)
    {
        services.TryAddSingleton<IKafkaProducerOptionsResolver, KafkaProducerOptionsResolver>();
    }

    internal static void TryAddStickedToKafkaConsumerKafkaProducerFactory(this IServiceCollection services)
    {
        services.TryAddSingleton<StickedToKafkaConsumerKafkaProducerFactory.ProducersStorage>();
        services.TryAddScoped<StickedToKafkaConsumerKafkaProducerFactory>();
    }
}
