using Confluent.Kafka;
using Polly;

namespace Abdm.Integration.MessageBroker.Kafka.Consumer;

internal class RetryConsumerMessageHandlerDecorator<TKey, TValue> : IConsumerMessageHandler<TKey, TValue>
{
    private readonly IConsumerMessageHandler<TKey, TValue> _handler;
    private readonly ResiliencePipeline _resiliencePipeline;

    public RetryConsumerMessageHandlerDecorator(
        IConsumerMessageHandler<TKey, TValue> handler,
        ResiliencePipeline resiliencePipeline)
    {
        _handler = handler;
        _resiliencePipeline = resiliencePipeline;
    }

    public async Task Handle(ConsumeResult<TKey, TValue> consumeResult)
    {
        await _resiliencePipeline.ExecuteAsync(async _ =>
        {
            await _handler.Handle(consumeResult);
        });
    }
}
