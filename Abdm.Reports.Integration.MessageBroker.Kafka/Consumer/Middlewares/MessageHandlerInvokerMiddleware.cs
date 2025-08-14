using Microsoft.Extensions.DependencyInjection;

namespace Abdm.Integration.MessageBroker.Kafka.Consumer.Middlewares;

public class MessageHandlerInvokerMiddleware<TKey, TMessage> : IKafkaConsumerMiddleware<TKey, TMessage>
{
    private readonly Type _messageHandlerType;

    public MessageHandlerInvokerMiddleware(Type messageHandlerType)
    {
        if (!messageHandlerType.IsAssignableTo(typeof(IKafkaMessageHandler<TKey, TMessage>)))
        {
            throw new InvalidOperationException(
                $"'{messageHandlerType.FullName}' is not assignable to " +
                $"{typeof(TKey).FullName}:{typeof(TMessage).FullName} kafka message handler");
        }

        _messageHandlerType = messageHandlerType;
    }

    public async Task Invoke(MessageContext<TKey, TMessage> context,
        HandleMessage<TKey, TMessage> next)
    {
        var handler = (IKafkaMessageHandler<TKey, TMessage>) context.ServiceProvider.GetRequiredService(_messageHandlerType);

        await handler.Handle(context.ConsumeResults.Single().Message.Value, context);
    }
}
