using System.ComponentModel.DataAnnotations;
using Confluent.Kafka;

namespace Abdm.Integration.MessageBroker.Kafka.Producer;

public class KafkaProducerOptions
{
    [Required(AllowEmptyStrings = false)]
    public string BootstrapServers { get; set; } = default!;

    [Required(AllowEmptyStrings = false)]
    public string Topic { get; set; } = default!;

    public Acks Acks { get; set; } = Acks.All;

    /// <summary>
    /// Timeout for message producing
    /// </summary>
    /// <example>
    /// "00:00:30.000" - 30s
    /// </example>
    public TimeSpan MessageTimeout { get; set; } = TimeSpan.FromSeconds(30);

    /// <summary>
    /// Max message size that can be sent to Kafka.
    /// Note: this is the client-side limitation, <c>max.message.bytes</c>
    /// should be also changed on the topic level.
    /// Default value is 1MB
    /// </summary>
    public int MessageMaxBytes { get; set; } = 1048588;

    /// <summary>
    /// Delay in milliseconds to wait for messages in the producer queue to accumulate
    /// before constructing message batches (MessageSets) to transmit to brokers
    /// </summary>
    /// <example>
    /// "00:00:00.050" - 50ms
    /// </example>
    public TimeSpan? Linger { get; set; }
}
