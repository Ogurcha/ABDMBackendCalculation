using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;

namespace Abdm.Integration.MessageBroker.Kafka.Consumer;

public class ScopedBatchConsumerMessageHandler<TKey, TValue> : IBatchConsumerMessageHandler<TKey, TValue>
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly HandleMessage<TKey, TValue> _handleMessage;

    public ScopedBatchConsumerMessageHandler(
        IServiceScopeFactory serviceScopeFactory,
        HandleMessage<TKey, TValue> handleMessage)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _handleMessage = handleMessage;
    }

    public async Task Handle(IReadOnlyCollection<ConsumeResult<TKey, TValue>> consumeResults)
    {
        using var scope = _serviceScopeFactory.CreateScope();

        var messageContext = new MessageContext<TKey, TValue>(
            serviceProvider: scope.ServiceProvider,
            consumeResult: consumeResults);

        await _handleMessage.Invoke(messageContext);
    }
}
