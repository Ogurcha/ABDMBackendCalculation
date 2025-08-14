using Microsoft.Extensions.DependencyInjection;

namespace Abdm.Integration.MessageBroker.Kafka.Consumer.Middlewares;

public class BatchMessageHandlerInvokerMiddleware<TKey, TValue> : IKafkaConsumerMiddleware<TKey, TValue>
{
    private readonly Type _messageHandlerType;

    public BatchMessageHandlerInvokerMiddleware(Type messageHandlerType)
    {
        if (!messageHandlerType.IsAssignableTo(typeof(IKafkaBatchMessageHandler<TKey, TValue>)))
        {
            throw new InvalidOperationException(
                $"'{messageHandlerType.FullName}' is not assignable to " +
                $"{typeof(TKey).FullName}:{typeof(TValue).FullName} kafka batch message handler");
        }

        _messageHandlerType = messageHandlerType;
    }

    public async Task Invoke(MessageContext<TKey, TValue> context, HandleMessage<TKey, TValue> next)
    {
        var handler = (IKafkaBatchMessageHandler<TKey, TValue>) context.ServiceProvider.GetRequiredService(_messageHandlerType);

        var messages = context.ConsumeResults
            .Select(cr => new KafkaMessage<TKey, TValue>(
                Key: cr.Message.Key,
                Value: cr.Message.Value))
            .ToArray();
        await handler.Handle(messages);
    }
}
