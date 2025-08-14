using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Abdm.Integration.MessageBroker.Kafka.Consumer.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddKafkaConsumer<TKey, TMessage>(
        this IServiceCollection services,
        Action<KafkaConsumerBuilder<TKey, TMessage>> builderAction)
    {
        return services.AddKafkaConsumer(
            consumerName: typeof(TMessage).Name,
            messageHandlerType: null,
            consumerBuilderAction: builderAction);
    }

    public static IServiceCollection AddKafkaConsumer<TKey, TMessage, TMessageHandler>(
        this IServiceCollection services,
        Action<KafkaConsumerBuilder<TKey, TMessage>> builderAction)
        where TMessageHandler : IKafkaMessageHandler<TKey, TMessage>
    {
        return services.AddKafkaConsumer(
            consumerName: typeof(TMessage).Name,
            messageHandlerType: typeof(TMessageHandler),
            consumerBuilderAction: builderAction);
    }

    public static IServiceCollection AddKafkaConsumer<TKey, TMessage, TMessageHandler>(
        this IServiceCollection services,
        string consumerName,
        Action<KafkaConsumerBuilder<TKey, TMessage>> builderAction)
        where TMessageHandler : IKafkaMessageHandler<TKey, TMessage>
    {
        return services.AddKafkaConsumer(
            consumerName: consumerName,
            messageHandlerType: typeof(TMessageHandler),
            consumerBuilderAction: builderAction);
    }

    public static IServiceCollection AddKafkaBatchConsumer<TKey, TMessage, TMessageHandler>(
        this IServiceCollection services,
        string consumerName,
        Action<KafkaConsumerBuilder<TKey, TMessage>> builderAction)
        where TMessageHandler : IKafkaBatchMessageHandler<TKey, TMessage>
    {
        return services.AddKafkaBatchConsumer(
            consumerName: consumerName,
            messageHandlerType: typeof(TMessageHandler),
            consumerBuilderAction: builderAction);
    }

    private static IServiceCollection AddKafkaConsumer<TKey, TMessage>(this IServiceCollection services,
        string consumerName, Type? messageHandlerType,
        Action<KafkaConsumerBuilder<TKey, TMessage>> consumerBuilderAction)
    {
        var kafkaConsumerBuilder = new KafkaConsumerBuilder<TKey, TMessage>(
            services: services,
            consumerBaseName: consumerName,
            messageHandlerType: messageHandlerType);

        consumerBuilderAction(kafkaConsumerBuilder);

        kafkaConsumerBuilder.RegisterConsumer();

        return services;
    }

    private static IServiceCollection AddKafkaBatchConsumer<TKey, TMessage>(this IServiceCollection services,
        string consumerName, Type? messageHandlerType,
        Action<KafkaConsumerBuilder<TKey, TMessage>> consumerBuilderAction)
    {
        var kafkaConsumerBuilder = new KafkaConsumerBuilder<TKey, TMessage>(
            services: services,
            consumerBaseName: consumerName,
            messageHandlerType: messageHandlerType);

        consumerBuilderAction(kafkaConsumerBuilder);

        kafkaConsumerBuilder.RegisterBatchConsumer();

        return services;
    }

    public static void TryAddKafkaConsumerOptionsResolver(this IServiceCollection services)
    {
        services.TryAddSingleton<IKafkaConsumerOptionsResolver, KafkaConsumerOptionsResolver>();
    }

    internal static void TryAddConsumerContext(this IServiceCollection services)
    {
        services.TryAddScoped<ConsumerContext>();
        services.TryAddScoped<IConsumerContext>(sp => sp.GetRequiredService<ConsumerContext>());
    }
}
