using Confluent.Kafka;
using Polly;

namespace Abdm.Integration.MessageBroker.Kafka.Consumer;

internal class RetryBatchConsumerMessageHandlerDecorator<TKey, TValue> : IBatchConsumerMessageHandler<TKey, TValue>
{
    private readonly IBatchConsumerMessageHandler<TKey, TValue> _handler;
    private readonly ResiliencePipeline _resiliencePipeline;

    public RetryBatchConsumerMessageHandlerDecorator(
        IBatchConsumerMessageHandler<TKey, TValue> handler,
        ResiliencePipeline resiliencePipeline)
    {
        _handler = handler;
        _resiliencePipeline = resiliencePipeline;
    }

    public async Task Handle(IReadOnlyCollection<ConsumeResult<TKey, TValue>> consumeResults)
    {
        await _resiliencePipeline.ExecuteAsync(async _ =>
        {
            await _handler.Handle(consumeResults);
        });
    }
}
