namespace Abdm.Integration.MessageBroker.Kafka.Consumer;

internal class ConsumerContext : IConsumerContext
{
    private string? _name;
    public string Name
    {
        get
        {
            if (_name is null)
            {
                throw new InvalidOperationException("Consumer context is not initialized");
            }

            return _name;
        }
    }

    internal void Initialize(string name)
    {
        _name = name;
    }
}
