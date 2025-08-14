using System.ComponentModel.DataAnnotations;
using Abdm.Integration.MessageBroker.Kafka.Consumer.Exceptions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Abdm.Integration.MessageBroker.Kafka.Consumer;

internal class KafkaConsumerOptionsResolver : IKafkaConsumerOptionsResolver
{
    private readonly IOptionsMonitor<KafkaConsumerOptions> _optionsMonitor;
    private readonly ILogger<KafkaConsumerOptionsResolver> _logger;

    public KafkaConsumerOptionsResolver(
        IOptionsMonitor<KafkaConsumerOptions> optionsMonitor,
        ILogger<KafkaConsumerOptionsResolver> logger)
    {
        _optionsMonitor = optionsMonitor;
        _logger = logger;
    }

    public KafkaConsumerOptions ResolveConsumerOptions(string consumerName)
    {
        var options = _optionsMonitor.Get(consumerName);

        var errors = new List<ValidationResult>();
        Validator.TryValidateObject(options, new ValidationContext(options), errors, true);

        if (errors.Any())
        {
            _logger.LogError("Kafka consumer {ConsumerName} is invalid ({@Errors})", consumerName, errors);

            throw new InvalidKafkaConsumerConfigurationException(
                consumerBaseName: consumerName,
                error: string.Join("; ", errors.Select(e => e.ErrorMessage)));
        }

        return options;
    }
}
