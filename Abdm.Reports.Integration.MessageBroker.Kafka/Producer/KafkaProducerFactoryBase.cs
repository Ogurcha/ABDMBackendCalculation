using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Abdm.Integration.MessageBroker.Kafka.Producer;

internal class KafkaProducerFactoryBase
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IKafkaProducerOptionsResolver _producerOptionsResolver;

    protected KafkaProducerFactoryBase(IServiceProvider serviceProvider,
        IKafkaProducerOptionsResolver producerOptionsResolver)
    {
        _serviceProvider = serviceProvider;
        _producerOptionsResolver = producerOptionsResolver;
    }

    protected KafkaProducer<TKey, TMessage> CreateProducer<TKey, TMessage>(string producerName)
    {
        var factoryOptions = GetProducerFactoryOptions<TKey, TMessage>(producerName);
        var producerOptions = _producerOptionsResolver.GetOptions(producerName);

        return new KafkaProducer<TKey, TMessage>(
            producerName: producerName,
            options: producerOptions,
            logger: _serviceProvider.GetRequiredService<ILogger<KafkaProducer<TKey, TMessage>>>(),
            keySerializer: factoryOptions.KeySerializerFactory?.Invoke(_serviceProvider),
            messageSerializer: factoryOptions.MessageSerializerFactory?.Invoke(_serviceProvider));
    }

    private KafkaProducerFactoryOptions<TKey, TMessage> GetProducerFactoryOptions<TKey, TMessage>(
        string producerName)
    {
        var factoryOptionsMonitor = _serviceProvider
            .GetRequiredService<IOptionsMonitor<KafkaProducerFactoryOptions<TKey, TMessage>>>();

        var options = factoryOptionsMonitor.Get(producerName);

        if (options.KeyType != typeof(TKey) || options.MessageType != typeof(TMessage))
        {
            throw new InvalidOperationException(
                $"Invalid key or message type requested for {producerName} producer");
        }

        return options;
    }
}
