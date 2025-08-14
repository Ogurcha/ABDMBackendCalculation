using System.ComponentModel;
using System.Diagnostics;
using Abdm.Integration.MessageBroker.Kafka.Consumer.Exceptions;
using Abdm.Integration.MessageBroker.Kafka.Consumer.Extensions;
using Abdm.Integration.MessageBroker.Kafka.Consumer.Middlewares;
using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Polly;

namespace Abdm.Integration.MessageBroker.Kafka.Consumer;

public class KafkaConsumerBuilder<TKey, TMessage>
{
    private readonly Type? _messageHandlerType;

    public string ConsumerBaseName { get; }

    public KafkaConsumerBuilder(IServiceCollection services,
        string consumerBaseName,
        Type? messageHandlerType)
    {
        Services = services;
        Configuration = new KafkaConsumerConfigurationBuilder<TKey, TMessage>(this);

        _messageHandlerType = messageHandlerType;
        ConsumerBaseName = consumerBaseName.ToLowerInvariant();
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public IServiceCollection Services { get; }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public List<Func<HandleMessage<TKey, TMessage>, HandleMessage<TKey, TMessage>>> HandlingPipeline { get; } = new ();

    [EditorBrowsable(EditorBrowsableState.Never)]
    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    public Func<IServiceProvider, IDeserializer<TKey>>? KeyDeserializerFactory { get; set; }

    [EditorBrowsable(EditorBrowsableState.Never)]
    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    public Func<IServiceProvider, IDeserializer<TMessage>>? MessageDeserializerFactory { get; set; }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public Func<IServiceProvider, IKafkaUnparsedMessageHandler> UnparsedMessageHandlerFactory { get; set; }
        = _ => new DefaultUnparsedMessageHandler();

    public KafkaConsumerConfigurationBuilder<TKey, TMessage> Configuration { get; }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public Func<IServiceProvider, ResiliencePipeline>? ResiliencePipelineFactory { get; set; }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public Func<IServiceProvider, IBatchConsumerLimiter<TKey, TMessage>>? BatchConsumerLimiterFactory { get; set; }

    // ReSharper disable once PropertyCanBeMadeInitOnly.Global
    public int ConsumersCount { get; set; } = 1;

    public KafkaConsumerBuilder<TKey, TMessage> UseMiddleware(
        Func<HandleMessage<TKey, TMessage>, HandleMessage<TKey, TMessage>> middleware)
    {
        HandlingPipeline.Add(middleware);

        return this;
    }

    public KafkaConsumerBuilder<TKey, TMessage> UseMiddleware(
        IKafkaConsumerMiddleware<TKey, TMessage> middleware)
    {
        return UseMiddleware(next =>
        {
            return async context =>
            {
                await middleware.Invoke(context, next);
            };
        });
    }

    public KafkaConsumerBuilder<TKey, TMessage> UseMiddleware<TMiddleware>()
        where TMiddleware : class, IKafkaConsumerMiddleware<TKey, TMessage>
    {
        Services.TryAddSingleton<TMiddleware>();

        return UseMiddleware(next =>
        {
            return async context =>
            {
                var middleware = context.ServiceProvider.GetRequiredService<TMiddleware>();

                await middleware.Invoke(context, next);
            };
        });
    }

    public KafkaConsumerBuilder<TKey, TMessage> UseBatchLimiter<TLimiter>()
        where TLimiter : class, IBatchConsumerLimiter<TKey, TMessage>
    {
        Services.TryAddSingleton<TLimiter>();

        BatchConsumerLimiterFactory = sp => sp.GetRequiredService<TLimiter>();

        return this;
    }

    internal void RegisterConsumer()
    {
        if (ConsumersCount < 1)
        {
            return;
        }

        RegisterConsumerBase();

        if (_messageHandlerType is not null)
        {
            Services.TryAddTransient(_messageHandlerType);
        }

        for (var i = 0; i < ConsumersCount; i++)
        {
            var consumerName = $"{ConsumerBaseName}-{i}";

            var handleMessage = BuildHandleMessageEntrypoint(
                consumerName: consumerName,
                entrypointMiddleware: _messageHandlerType is null
                    ? null
                    : new MessageHandlerInvokerMiddleware<TKey, TMessage>(_messageHandlerType));

            Services.AddSingleton<IKafkaConsumer>(sp =>
            {
                var consumerMessageHandlerFactory = sp.GetRequiredService<IConsumerMessageHandlerFactory<TKey, TMessage>>();
                var consumerMessageHandler = consumerMessageHandlerFactory.Create(handleMessage);

                if (ResiliencePipelineFactory is not null)
                {
                    var resiliencePipeline = ResiliencePipelineFactory(sp);
                    consumerMessageHandler = new RetryConsumerMessageHandlerDecorator<TKey, TMessage>(
                        handler: consumerMessageHandler,
                        resiliencePipeline: resiliencePipeline);
                }

                var optionsResolver = sp.GetRequiredService<IKafkaConsumerOptionsResolver>();

                return new KafkaConsumer<TKey, TMessage>(consumerName: consumerName,
                    consumerBaseName: ConsumerBaseName,
                    options: optionsResolver.ResolveConsumerOptions(ConsumerBaseName),
                    messageHandler: consumerMessageHandler,
                    logger: sp.GetRequiredService<ILogger<KafkaConsumer<TKey, TMessage>>>(),
                    keyDeserializer: KeyDeserializerFactory?.Invoke(sp),
                    messageDeserializer: MessageDeserializerFactory?.Invoke(sp),
                    unparsedMessageHandler: UnparsedMessageHandlerFactory(sp));
            });
        }
    }

    internal void RegisterBatchConsumer()
    {
        if (ConsumersCount < 1)
        {
            return;
        }

        RegisterConsumerBase();
        Services.TryAddSingleton<DefaultBatchConsumerLimiter<TKey, TMessage>>();

        if (_messageHandlerType is not null)
        {
            Services.TryAddTransient(_messageHandlerType);
        }

        for (var i = 0; i < ConsumersCount; i++)
        {
            var consumerName = $"{ConsumerBaseName}-{i}";

            var handleMessage = BuildHandleMessageEntrypoint(
                consumerName: consumerName,
                entrypointMiddleware: _messageHandlerType is null
                    ? null
                    : new BatchMessageHandlerInvokerMiddleware<TKey, TMessage>(_messageHandlerType));

            Services.AddSingleton<IKafkaConsumer>(sp =>
            {
                var consumerMessageHandlerFactory = sp.GetRequiredService<IConsumerMessageHandlerFactory<TKey, TMessage>>();
                var consumerMessageHandler = consumerMessageHandlerFactory.CreateBatch(handleMessage);

                if (ResiliencePipelineFactory is not null)
                {
                    var resiliencePipeline = ResiliencePipelineFactory(sp);
                    consumerMessageHandler = new RetryBatchConsumerMessageHandlerDecorator<TKey, TMessage>(
                        handler: consumerMessageHandler,
                        resiliencePipeline: resiliencePipeline);
                }

                var optionsResolver = sp.GetRequiredService<IKafkaConsumerOptionsResolver>();

                return new KafkaBatchConsumer<TKey, TMessage>(consumerName: consumerName,
                    consumerBaseName: ConsumerBaseName,
                    options: optionsResolver.ResolveConsumerOptions(ConsumerBaseName),
                    messageHandler: consumerMessageHandler,
                    logger: sp.GetRequiredService<ILogger<KafkaConsumer<TKey, TMessage>>>(),
                    keyDeserializer: KeyDeserializerFactory?.Invoke(sp),
                    messageDeserializer: MessageDeserializerFactory?.Invoke(sp),
                    unparsedMessageHandler: UnparsedMessageHandlerFactory(sp),
                    limiter: BatchConsumerLimiterFactory is null
                        ? sp.GetRequiredService<DefaultBatchConsumerLimiter<TKey, TMessage>>()
                        : BatchConsumerLimiterFactory(sp));
            });
        }
    }

    private void RegisterConsumerBase()
    {
        ReserveConsumer();

        if (ConsumersCount < 1)
        {
            throw new InvalidKafkaConsumerConfigurationException(
                consumerBaseName: ConsumerBaseName,
                error: $"{nameof(ConsumersCount)} must be greater than 0");
        }

        Services.TryAddKafkaConsumerOptionsResolver();
        var optionsBuilder = Services.AddOptions<KafkaConsumerOptions>(ConsumerBaseName);

        foreach (var optionsBuilderAction in Configuration.OptionsBuilderActions)
        {
            optionsBuilderAction(optionsBuilder);
        }

        Services.AddHostedService<KafkaConsumerHostedService>();
        Services.TryAddSingleton<IConsumerMessageHandlerFactory<TKey, TMessage>, ScopedConsumerMessageHandlerFactory<TKey, TMessage>>();
        Services.TryAddConsumerContext();
    }

    private void ReserveConsumer()
    {
        Services.TryAddSingleton(new KafkaConsumerRegistry());

        var registry = (KafkaConsumerRegistry)Services
            .Single(sd => sd.ServiceType == typeof(KafkaConsumerRegistry)).ImplementationInstance!;
        Debug.Assert(registry != null);

        if (!registry.RegisteredConsumerBaseNames.Add(ConsumerBaseName))
        {
            throw new InvalidKafkaConsumerConfigurationException(ConsumerBaseName,
                $"Kafka consumer with the name '{ConsumerBaseName}' already registered. Specify a different name.");
        }
    }

    private HandleMessage<TKey, TMessage> BuildHandleMessageEntrypoint(
        string consumerName,
        IKafkaConsumerMiddleware<TKey, TMessage>? entrypointMiddleware)
    {
        if (entrypointMiddleware is not null)
        {
            UseMiddleware(entrypointMiddleware);
        }

        HandleMessage<TKey, TMessage> entrypoint = _ => Task.CompletedTask;

        foreach (var middleware in HandlingPipeline.AsEnumerable().Reverse())
        {
            entrypoint = middleware(entrypoint);
        }

        var entrypoint1 = entrypoint;
        entrypoint = async context =>
        {
            var consumerContext = context.ServiceProvider.GetRequiredService<ConsumerContext>();
            consumerContext.Initialize(consumerName);

            await entrypoint1(context);
        };

        return entrypoint;
    }
}
