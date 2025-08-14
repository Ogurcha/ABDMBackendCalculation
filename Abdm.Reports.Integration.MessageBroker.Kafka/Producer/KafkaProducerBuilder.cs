using System.ComponentModel;
using System.Diagnostics;
using Abdm.Integration.MessageBroker.Kafka.Producer.Exceptions;
using Abdm.Integration.MessageBroker.Kafka.Producer.Extensions;
using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

// ReSharper disable RedundantTypeArgumentsOfMethod

namespace Abdm.Integration.MessageBroker.Kafka.Producer;

public class KafkaProducerBuilder
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public Func<IServiceProvider, IKafkaProducerFactory> KafkaProducerFactoryFactory { get; set; }
        = sp => sp.GetRequiredService<IKafkaProducerFactory>();

    [EditorBrowsable(EditorBrowsableState.Never)]
    public IServiceCollection Services { get; protected set; }
}

public class KafkaProducerBuilder<TKey, TMessage> : KafkaProducerBuilder
{
    public KafkaProducerBuilder(IServiceCollection services, string producerName)
    {
        Services = services;
        Configuration = new KafkaProducerConfigurationBuilder<TKey, TMessage>(this);
        ProducerName = producerName.ToLowerInvariant();
    }

    public KafkaProducerConfigurationBuilder<TKey, TMessage> Configuration { get; }

    public string ProducerName { get; }

    // ReSharper disable once PropertyCanBeMadeInitOnly.Global
    public bool RegisterInServices { get; set; } = true;

    public Func<IServiceProvider, ISerializer<TKey>>? KeySerializerFactory { get; set; }

    public Func<IServiceProvider, ISerializer<TMessage>>? MessageSerializerFactory { get; set; }

    // ReSharper disable once StaticMemberInGenericType
    private static readonly IReadOnlyDictionary<Type, object> _defaultSerializers = new Dictionary<Type, object>
    {
        { typeof(Null), Serializers.Null },
        { typeof(int), Serializers.Int32 },
        { typeof(long), Serializers.Int64 },
        { typeof(string), Serializers.Utf8 },
        { typeof(float), Serializers.Single },
        { typeof(double), Serializers.Double },
        { typeof(byte[]), Serializers.ByteArray }
    };

    internal void RegisterProducer()
    {
        Services.TryAddSingleton<IKafkaProducerFactory, DefaultKafkaProducerFactory>();

        ReserveProducerName(ProducerName);
        RegisterProducerOptions(ProducerName);
        RegisterProducerFactoryOptions<TKey, TMessage>(ProducerName, o =>
        {
            o.KeySerializerFactory = KeySerializerFactory;
            o.MessageSerializerFactory = MessageSerializerFactory;
            o.KeyType = typeof(TKey);
            o.MessageType = typeof(TMessage);
        });

        if (RegisterInServices)
        {
            CheckAlreadyRegisteredInServices(ProducerName);

            Services.AddScoped<IKafkaProducer<TKey, TMessage>>(sp =>
            {
                var producerFactory = KafkaProducerFactoryFactory(sp);
                return producerFactory.Create<TKey, TMessage>(ProducerName);
            });
        }
    }
    
    private void RegisterProducerFactoryOptions<TProducerKey, TProducerMessage>(string producerName,
        Action<KafkaProducerFactoryOptions<TProducerKey, TProducerMessage>> configure)
    {
        Services
            .AddOptions<KafkaProducerFactoryOptions<TProducerKey, TProducerMessage>>(producerName)
            .ValidateDataAnnotations()
            .ValidateOnStart()
            .Configure(configure);
    }

    private void CheckAlreadyRegisteredInServices(string producerName)
    {
        if (Services.Any(sd => sd.ServiceType == typeof(IKafkaProducer<TKey, TMessage>)))
        {
            throw new InvalidKafkaProducerConfigurationException(producerName,
                $"Producer with the key type {typeof(TKey).FullName} and the message type {typeof(TMessage).FullName} already registered." +
                $"Use configuration builder property {nameof(RegisterInServices)} to configure producer registration.");
        }
    }

    private void ReserveProducerName(string producerName)
    {
        Services.TryAddSingleton(new KafkaProducersRegistry());

        var registry = (KafkaProducersRegistry)Services
            .Single(sd => sd.ServiceType == typeof(KafkaProducersRegistry)).ImplementationInstance!;
        Debug.Assert(registry != null);

        if (registry.RegisteredNames.Contains(producerName))
        {
            throw new InvalidOperationException($"Kafka producer {producerName} already registered");
        }

        registry.RegisteredNames.Add(producerName);
    }

    private void RegisterProducerOptions(string producerName)
    {
        var optionsBuilder = Services.AddOptions<KafkaProducerOptions>(producerName);
        foreach (var optionsBuilderAction in Configuration.OptionsBuilderActions)
        {
            optionsBuilderAction(optionsBuilder);
        }

        Services.TryAddKafkaProducerOptionsResolver();
    }

    private class KafkaProducersRegistry
    {
        public HashSet<string> RegisteredNames { get; } = new ();
    }
}
