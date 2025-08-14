using System.ComponentModel.DataAnnotations;
using Confluent.Kafka;

namespace Abdm.Integration.MessageBroker.Kafka.Consumer;

public class KafkaConsumerOptions
{
    [Required(AllowEmptyStrings = false)]
    public string Topic { get; set; } = default!;

    [Required(AllowEmptyStrings = false)]
    public string ConsumerGroup { get; set; } = default!;

    [Required(AllowEmptyStrings = false)]
    public string BootstrapServers { get; set; } = default!;

    public int SessionTimeoutMs { get; set; } = 45000;

    public int MaxPollIntervalMs { get; set; } = 300000;

    public int? QueuedMinMessages { get; set; } = 100;

    public int? QueuedMaxMessagesKbytes { get; set; } = 20000;

    public AutoOffsetReset AutoOffsetReset { get; set; } = AutoOffsetReset.Earliest;

    public TimeSpan WaitForMessage { get; set; } = TimeSpan.FromMilliseconds(100);

    public TimeSpan DelayAfterEndOfPartition { get; set; } = TimeSpan.FromMilliseconds(100);

    public TimeSpan WaitForReconnect { get; set; } = TimeSpan.FromMilliseconds(5000);

    public bool EnableAutoCommit { get; set; } = false;

    public int MaxBatchSize { get; set; } = 100;

    public TimeSpan MaxWaitForBatch { get; set; } = TimeSpan.FromMilliseconds(100);

    public SecurityProtocol? SecurityProtocol { get; set; }

    public KafkaConsumerSaslOptions? Sasl { get; set; }

    public bool? EnableSslCertificateVerification { get; set; }

    public PartitionAssignmentStrategy RebalancingStrategy { get; set; } = PartitionAssignmentStrategy.Range;
}