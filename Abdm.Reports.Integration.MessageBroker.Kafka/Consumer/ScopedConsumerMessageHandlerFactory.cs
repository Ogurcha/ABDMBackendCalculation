using Microsoft.Extensions.DependencyInjection;

namespace Abdm.Integration.MessageBroker.Kafka.Consumer;

internal class ScopedConsumerMessageHandlerFactory<TKey, TMessage>
    : IConsumerMessageHandlerFactory<TKey, TMessage>
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public ScopedConsumerMessageHandlerFactory(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    public IConsumerMessageHandler<TKey, TMessage> Create(HandleMessage<TKey, TMessage> handleMessage)
    {
        return new ScopedConsumerMessageHandler<TKey, TMessage>(_serviceScopeFactory, handleMessage);
    }

    public IBatchConsumerMessageHandler<TKey, TMessage> CreateBatch(HandleMessage<TKey, TMessage> handleMessage)
    {
        return new ScopedBatchConsumerMessageHandler<TKey, TMessage>(_serviceScopeFactory, handleMessage);
    }
}
