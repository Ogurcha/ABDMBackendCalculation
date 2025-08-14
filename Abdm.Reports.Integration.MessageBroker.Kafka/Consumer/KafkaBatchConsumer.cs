using System.Diagnostics;
using Confluent.Kafka;
using Microsoft.Extensions.Logging;

namespace Abdm.Integration.MessageBroker.Kafka.Consumer;

internal class KafkaBatchConsumer<TKey, TMessage> : KafkaConsumerBase<TKey, TMessage>
{
    private readonly IBatchConsumerMessageHandler<TKey, TMessage> _messageHandler;
    private readonly IBatchConsumerLimiter<TKey, TMessage> _limiter;

    public KafkaBatchConsumer(string consumerName,
        string consumerBaseName,
        KafkaConsumerOptions options,
        IBatchConsumerMessageHandler<TKey, TMessage> messageHandler,
        ILogger<KafkaConsumerBase<TKey, TMessage>> logger,
        IDeserializer<TKey>? keyDeserializer,
        IDeserializer<TMessage>? messageDeserializer,
        IKafkaUnparsedMessageHandler unparsedMessageHandler,
        IBatchConsumerLimiter<TKey, TMessage> limiter)
        : base(consumerName: consumerName,
            consumerBaseName: consumerBaseName,
            options: options,
            logger: logger,
            keyDeserializer: keyDeserializer,
            messageDeserializer: messageDeserializer,
            unparsedMessageHandler: unparsedMessageHandler)
    {
        _messageHandler = messageHandler;
        _limiter = limiter;
    }

    protected override async Task Consume(IConsumer<TKey, TMessage> consumer, CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            var consumeResults = await ConsumeBatch(consumer, cancellationToken);

            if (consumeResults.Count == 0)
            {
                await Task.Delay(_options.DelayAfterEndOfPartition, cancellationToken);
                continue;
            }

            await HandleKafkaMessages(consumeResults);

            if (!_options.EnableAutoCommit)
            {
                var offsetsByTopicPartition = new Dictionary<TopicPartition, TopicPartitionOffset>();
                foreach (var consumeResult in consumeResults)
                {
                    offsetsByTopicPartition[consumeResult.TopicPartition] = consumeResult.TopicPartitionOffset;
                }

                var offsets = offsetsByTopicPartition.Values
                    .Select(tpo => new TopicPartitionOffset(tpo.TopicPartition, tpo.Offset + 1, tpo.LeaderEpoch))
                    .ToArray();
                CommitOffsets(consumer, offsets);
            }

            _logger.LogInformation("Kafka messages batch handled ({MessagesCount}, {ConsumerName})",
                consumeResults.Count, _consumerName);
        }
    }

    private async Task<List<ConsumeResult<TKey, TMessage>>> ConsumeBatch(
        IConsumer<TKey, TMessage> consumer, CancellationToken cancellationToken)
    {
        var consumeResults = new List<ConsumeResult<TKey, TMessage>>();
        var sw = Stopwatch.StartNew();
        using var cts = new CancellationTokenSource(_options.MaxWaitForBatch);

        while (!cancellationToken.IsCancellationRequested && !cts.IsCancellationRequested)
        {
            ConsumeResult<TKey, TMessage>? consumeResult;

            try
            {
                consumeResult = consumer.Consume(_options.WaitForMessage);
            }
            catch (ConsumeException e) when (e.Error.Code == ErrorCode.Local_KeyDeserialization)
            {
                await KeyDeserializationFailed(e);
                _logger.LogInformation(
                    "Message skipped at {Offset} in {Partition} partition of {Topic} topic by {ConsumerName} (message key deserialization failed)",
                    e.ConsumerRecord.Offset.Value, e.ConsumerRecord.Partition.Value, e.ConsumerRecord.Topic,
                    _consumerName);
                continue;
            }
            catch (ConsumeException e) when (e.Error.Code == ErrorCode.Local_ValueDeserialization)
            {
                await ValueDeserializationFailed(e);
                _logger.LogInformation(
                    "Message skipped at {Offset} in {Partition} partition of {Topic} topic by {ConsumerName} (message value deserialization failed)",
                    e.ConsumerRecord.Offset.Value, e.ConsumerRecord.Partition.Value, e.ConsumerRecord.Topic,
                    _consumerName);
                continue;
            }

            if (consumeResult == null)
            {
                if (consumeResults.Any())
                {
                    _logger.LogInformation("Kafka messages batch completed (empty consume result, {MessagesCount})", consumeResults.Count);
                }

                return consumeResults;
            }

            _logger.LogInformation("Message consumed at {Offset} in {Partition} partition of {Topic} topic by {ConsumerName}",
                consumeResult.Offset.Value, consumeResult.Partition.Value, consumeResult.Topic, _consumerName);

            consumeResults.Add(consumeResult);

            if (consumeResults.Count >= _options.MaxBatchSize)
            {
                _logger.LogInformation("Kafka messages batch completed (max batch size reached, {MessagesCount}, {Elapsed})",
                    consumeResults.Count, sw.Elapsed);
                return consumeResults;
            }

            if (_limiter.NeedToCompleteBatch(consumeResults))
            {
                _logger.LogInformation("Kafka messages batch completed (batch limiter, {MessagesCount}, {Elapsed})",
                    consumeResults.Count, sw.Elapsed);
                return consumeResults;
            }
        }

        _logger.LogInformation("Kafka messages batch completed (timeout, {MessagesCount}, {Elapsed})",
            consumeResults.Count, sw.Elapsed);
        return consumeResults;
    }

    private IReadOnlyCollection<ConsumeResult<TKey, TMessage>> NormalizeMessages(
        IReadOnlyCollection<ConsumeResult<TKey, TMessage>> consumeResults)
    {
        if (!consumeResults.Any(cr => cr.Message.Value is null))
        {
            return consumeResults;
        }

        var newConsumeResults = new List<ConsumeResult<TKey, TMessage>>(consumeResults.Count);
        foreach (var consumeResult in consumeResults)
        {
            if (consumeResult.Message.Value is null)
            {
                _logger.LogError("Kafka message value is null, message at {Offset} in {Partition} partition of {Topic} topic skipped",
                    consumeResult.Offset.Value, consumeResult.Partition.Value, consumeResult.Topic);
            }
            else
            {
                newConsumeResults.Add(consumeResult);
            }
        }

        return newConsumeResults;
    }

    private async Task HandleKafkaMessages(IReadOnlyCollection<ConsumeResult<TKey, TMessage>> consumeResults)
    {
        consumeResults = NormalizeMessages(consumeResults);
        if (consumeResults.Count == 0)
        {
            return;
        }

        try
        {
            await _messageHandler.Handle(consumeResults);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Exception while handling messages ({ConsumerName}, {MessagesCount})",
                _consumerName, consumeResults.Count);

            throw;
        }

        foreach (var consumeResult in consumeResults)
        {
            _logger.LogInformation(
                "Message handled at {Offset} in {Partition} partition of {Topic} topic by {ConsumerName}",
                consumeResult.Offset.Value, consumeResult.Partition.Value, consumeResult.Topic, _consumerName);
        }
    }
}
