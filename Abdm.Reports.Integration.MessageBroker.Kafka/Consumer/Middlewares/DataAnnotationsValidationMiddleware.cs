using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Abdm.Integration.MessageBroker.Kafka.Consumer.Middlewares;

internal class DataAnnotationsValidationMiddleware<TKey, TMessage> : IKafkaConsumerMiddleware<TKey, TMessage>
{
    private readonly string _consumerBaseName;

    public DataAnnotationsValidationMiddleware(string consumerBaseName)
    {
        _consumerBaseName = consumerBaseName;
    }
    
    public Task Invoke(MessageContext<TKey, TMessage> context, HandleMessage<TKey, TMessage> next)
    {
        foreach (var consumeResult in context.ConsumeResults)
        {
            if (consumeResult.Message.Value == null)
            {
                var logger = context.ServiceProvider.GetRequiredService<ILogger<DataAnnotationsValidationMiddleware<TKey, TMessage>>>();

                logger.LogError("Kafka message value is null");

                return Task.CompletedTask;
            }

            var target = consumeResult.Message.Value;
            List<ValidationResult> validationResultList = new List<ValidationResult>();
            Validator.TryValidateObject(target, new ValidationContext(target), validationResultList, true);
            var errors = (IReadOnlyCollection<ValidationResult>) validationResultList;
            
            if (errors.Any())
            {
                var logger = context.ServiceProvider.GetRequiredService<ILogger<DataAnnotationsValidationMiddleware<TKey, TMessage>>>();

                logger.LogError("Message value validation failed {@Message} ({@Errors})", consumeResult.Message.Value, errors);

                return Task.CompletedTask;
            }
        }

        return next(context);
    }
}
