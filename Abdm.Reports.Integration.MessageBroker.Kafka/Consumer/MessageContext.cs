using Confluent.Kafka;

namespace Abdm.Integration.MessageBroker.Kafka.Consumer;

public class MessageContext<TKey, TMessage>
{
    public MessageContext(IServiceProvider serviceProvider,
        IReadOnlyCollection<ConsumeResult<TKey, TMessage>> consumeResult)
    {
        ServiceProvider = serviceProvider;
        ConsumeResults = consumeResult;
    }

    public IServiceProvider ServiceProvider { get; }

    public IReadOnlyCollection<ConsumeResult<TKey, TMessage>> ConsumeResults { get; }
}