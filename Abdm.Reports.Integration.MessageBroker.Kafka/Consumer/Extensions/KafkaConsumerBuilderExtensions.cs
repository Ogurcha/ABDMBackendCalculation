using Abdm.Integration.MessageBroker.Kafka.Consumer.Middlewares;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Registry;

namespace Abdm.Integration.MessageBroker.Kafka.Consumer.Extensions;

public static partial class KafkaConsumerBuilderExtensions
{
    public static KafkaConsumerBuilder<TKey, TMessage> UseDataAnnotationsValidationMiddleware<TKey, TMessage>(
        this KafkaConsumerBuilder<TKey, TMessage> builder)
    {
        builder.UseMiddleware(new DataAnnotationsValidationMiddleware<TKey, TMessage>(builder.ConsumerBaseName));
        return builder;
    }

    public static KafkaConsumerBuilder<TKey, TValue> UseRetryResiliencePipeline<TKey, TValue>(
        this KafkaConsumerBuilder<TKey, TValue> builder,
        Func<IServiceProvider, ResiliencePipeline> resiliencePipelineFactory)
    {
        builder.ResiliencePipelineFactory = resiliencePipelineFactory;

        return builder;
    }

    public static KafkaConsumerBuilder<TKey, TValue> UseRetryResiliencePipeline<TKey, TValue>(
        this KafkaConsumerBuilder<TKey, TValue> builder,
        ResiliencePipeline resiliencePipeline)
    {
        builder.ResiliencePipelineFactory = _ => resiliencePipeline;

        return builder;
    }

    public static KafkaConsumerBuilder<TKey, TValue> UseRetryResiliencePipeline<TKey, TValue>(
        this KafkaConsumerBuilder<TKey, TValue> builder,
        string pipelineName)
    {
        builder.ResiliencePipelineFactory = sp =>
        {
            var resiliencePipelineProvider = sp.GetRequiredService<ResiliencePipelineProvider<string>>();
            return resiliencePipelineProvider.GetPipeline(pipelineName);
        };

        return builder;
    }
}
