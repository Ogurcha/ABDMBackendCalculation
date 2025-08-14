using System.ComponentModel.DataAnnotations;
using Confluent.Kafka;

namespace Abdm.Integration.MessageBroker.Kafka.Producer;

internal class KafkaProducerFactoryOptions<TKey, TMessage>
{
    public Func<IServiceProvider, ISerializer<TKey>>? KeySerializerFactory { get; set; }

    public Func<IServiceProvider, ISerializer<TMessage>>? MessageSerializerFactory { get; set; }

    [Required]
    public Type KeyType { get; set; } = default!;

    [Required]
    public Type MessageType { get; set; } = default!;
}
