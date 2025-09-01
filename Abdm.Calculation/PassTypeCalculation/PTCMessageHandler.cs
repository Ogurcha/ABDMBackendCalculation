using System;
using System.Threading.Tasks;
using Abdm.Calculation.PassTypeCalculation.DTO;
using Kafka.Integration.MessageBroker.Consumer;
using Kafka.Integration.MessageBroker.Producer;
using Microsoft.Extensions.Logging;

namespace Abdm.Calculation.ColumnCalculation
{

    /// <summary>
    /// Хендлер для сообщений из брокера.
    /// Рассчет условий пропуска
    /// </summary>
    public class PTCMessageHandler(
        IPassTypeCalculator ptcProcessor, 
        ILogger<PTCMessageHandler> logger,
        IKafkaProducer<string, PTCResultMessage> messageProducer
        ) : IKafkaMessageHandler<string, PTCRequestMessage>
    {
        public async Task Handle(PTCRequestMessage message, MessageContext<string, PTCRequestMessage> context)
        {
            PTCResultMessage responseContent = null;
            try
            {
                logger.LogInformation($"PassType calculation for (IssoId = {message.IssoId}, Check point number = {message.CPNumber}) started");
                responseContent = await ptcProcessor.CalculatePassType(message);
                await messageProducer.Produce(responseContent.GetBrokerId, responseContent);
            }
            catch (Exception e)
            {
                logger.LogError(e, $"PassType calculation failed (IssoId = {message?.IssoId}, Check point number = {message?.CPNumber})");
                if (responseContent != null && responseContent.IssoId > 0 && responseContent.CPNumber > 0)
                {
                    try
                    {
                        await messageProducer.Produce(responseContent.GetBrokerId, responseContent);
                    }
                    catch {
                        logger.LogError(e, $"Message producer failed to send message");
                    }
                }
            }
        }
    }
}
