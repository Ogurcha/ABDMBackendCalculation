using Confluent.Kafka;
using Microsoft.Extensions.Logging;

namespace Abdm.Integration.MessageBroker.Kafka.Consumer;

internal class KafkaConsumer<TKey, TMessage> : KafkaConsumerBase<TKey, TMessage>
{
    private readonly IConsumerMessageHandler<TKey, TMessage> _messageHandler;

    public KafkaConsumer(
        string consumerName,
        string consumerBaseName,
        KafkaConsumerOptions options,
        IConsumerMessageHandler<TKey, TMessage> messageHandler,
        ILogger<KafkaConsumerBase<TKey, TMessage>> logger,
        IDeserializer<TKey>? keyDeserializer,
        IDeserializer<TMessage>? messageDeserializer,
        IKafkaUnparsedMessageHandler unparsedMessageHandler)
        : base(consumerName: consumerName,
            consumerBaseName: consumerBaseName,
            options: options,
            logger: logger,
            keyDeserializer: keyDeserializer,
            messageDeserializer: messageDeserializer,
            unparsedMessageHandler: unparsedMessageHandler)
    {
        _messageHandler = messageHandler;
    }

    protected override async Task Consume(
        IConsumer<TKey, TMessage> consumer,
        CancellationToken cancellationToken)
    {
        ConsumeResult<TKey, TMessage>? consumeResultToCommit = null;

        while (!cancellationToken.IsCancellationRequested)
        {
            ConsumeResult<TKey, TMessage>? consumeResult;

            try
            {
                consumeResult = consumer.Consume(_options.WaitForMessage);
            }
            catch (ConsumeException e) when (e.Error.Code == ErrorCode.Local_KeyDeserialization)
            {
                await KeyDeserializationFailed(e);

                CommitOffset(consumer, e.ConsumerRecord);

                _logger.LogInformation("Message skipped at {Offset} in {Partition} partition of {Topic} topic by {ConsumerName} (message key deserialization failed)",
                    e.ConsumerRecord.Offset.Value, e.ConsumerRecord.Partition.Value, e.ConsumerRecord.Topic, _consumerName);
                break;
            }
            catch (ConsumeException e) when (e.Error.Code == ErrorCode.Local_ValueDeserialization)
            {
                await ValueDeserializationFailed(e);

                CommitOffset(consumer, e.ConsumerRecord);

                _logger.LogInformation("Message skipped at {Offset} in {Partition} partition of {Topic} topic by {ConsumerName} (message value deserialization failed)",
                    e.ConsumerRecord.Offset.Value, e.ConsumerRecord.Partition.Value, e.ConsumerRecord.Topic, _consumerName);
                break;
            }

            if (consumeResult == null)
            {
                await Task.Delay(_options.DelayAfterEndOfPartition, cancellationToken);
                continue;
            }
            
            _logger.LogInformation("Message consumed at {Offset} in {Partition} partition of {Topic} topic by {ConsumerName}",
                consumeResult.Offset.Value, consumeResult.Partition.Value, consumeResult.Topic, _consumerName);

            await HandleKafkaMessage(consumeResult);
            consumeResultToCommit = consumeResult;

            if (!_options.EnableAutoCommit)
            {
                CommitOffset(consumer, consumeResultToCommit);
                consumeResultToCommit = null;
            }
        }

        if (!_options.EnableAutoCommit
            && consumeResultToCommit is not null)
        {
            CommitOffset(consumer, consumeResultToCommit);
        }
    }

    private async Task HandleKafkaMessage(ConsumeResult<TKey, TMessage> consumeResult)
    {
        if (consumeResult.Message.Value is null)
        {
            _logger.LogError("Kafka message value is null, message at {Offset} in {Partition} partition of {Topic} topic skipped",
                consumeResult.Offset.Value, consumeResult.Partition.Value, consumeResult.Topic);
            return;
        }
        
        try
        {
            await _messageHandler.Handle(consumeResult);

            _logger.LogInformation(
                "Message handled at {Offset} in {Partition} partition of {Topic} topic by {ConsumerName}",
                consumeResult.Offset.Value, consumeResult.Partition.Value, consumeResult.Topic, _consumerName);
        }
        catch (Exception e)
        {
            _logger.LogError(e,
                "Exception while handling message {@Message} at {Offset} offset in {Partition} partition of {Topic} topic by {ConsumerName}",
                consumeResult.Message, consumeResult.Offset.Value, consumeResult.Partition.Value,
                consumeResult.Topic, _consumerName);

            throw;
        }
    }
}
