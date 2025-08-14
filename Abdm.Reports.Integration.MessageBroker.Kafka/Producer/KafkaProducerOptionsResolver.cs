using System.ComponentModel.DataAnnotations;
using Abdm.Integration.MessageBroker.Kafka.Producer.Exceptions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Abdm.Integration.MessageBroker.Kafka.Producer;

internal class KafkaProducerOptionsResolver : IKafkaProducerOptionsResolver
{
    private readonly IOptionsMonitor<KafkaProducerOptions> _optionsMonitor;
    private readonly ILogger<KafkaProducerOptionsResolver> _logger;

    public KafkaProducerOptionsResolver(
        IOptionsMonitor<KafkaProducerOptions> optionsMonitor,
        ILogger<KafkaProducerOptionsResolver> logger)
    {
        _optionsMonitor = optionsMonitor;
        _logger = logger;
    }

    public KafkaProducerOptions GetOptions(string producerName)
    {
        var producerOptions = _optionsMonitor.Get(producerName);

        var errors = new List<ValidationResult>();
        Validator.TryValidateObject(producerOptions,
            new ValidationContext(producerOptions), errors, true);

        if (errors.Any())
        {
            _logger.LogError("Kafka producer {ProducerName} is invalid ({@Errors})", producerName, errors);

            throw new InvalidKafkaProducerConfigurationException(
                producerName: producerName,
                error: string.Join("; ", errors.Select(e => e.ErrorMessage)));
        }

        return producerOptions;
    }
}
