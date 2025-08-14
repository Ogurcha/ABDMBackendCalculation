using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;

namespace Abdm.Integration.MessageBroker.Kafka.Consumer;

internal class ScopedConsumerMessageHandler<TKey, TMessage> : IConsumerMessageHandler<TKey, TMessage>
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly HandleMessage<TKey, TMessage> _handleMessage;

    public ScopedConsumerMessageHandler(IServiceScopeFactory serviceScopeFactory,
        HandleMessage<TKey, TMessage> handleMessage)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _handleMessage = handleMessage;
    }

    public async Task Handle(ConsumeResult<TKey, TMessage> consumeResult)
    {
        using var scope = _serviceScopeFactory.CreateScope();

        var messageContext = new MessageContext<TKey, TMessage>(
            serviceProvider: scope.ServiceProvider,
            consumeResult: new []
            {
                consumeResult
            });

        await _handleMessage.Invoke(messageContext);
    }
}
