using System;
using System.Threading.Tasks;
using Abdm.Calculation.BLL.Models;
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
        private const string infoMsg = "PassType calculation for (IssoId = {1}, Check point number = {2}) started";
        private const string errorMsg = "Failed PassType calculation for (IssoId = {1}, Check point number = {2})";

        public async Task Handle(PTCRequestMessage message, MessageContext<string, PTCRequestMessage> context)
        {
            PTCResultMessage responseContent = null;
            try
            {
                logger.LogInformation(string.Format(infoMsg, message.IssoId, message.CPNumber));
                responseContent = await ptcProcessor.CalculatePassType(message);
                await messageProducer.Produce(responseContent.GetBrokerId, responseContent);
            }
            catch (Exception e)
            {
                logger.LogError(string.Format(errorMsg, message?.IssoId, message?.CPNumber));
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
