using System;
using System.Threading.Tasks;
using Abdm.Calculation.BLL.Models;
using Abdm.Calculation.WebApi.Mappers;
using Abdm.Calculation.WebApi.RequestModels;
using Abdm.Calculation.WebApi.ResponseModels;
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
        IKafkaProducer<string, PTCResultMessageResponseModel> messageProducer,
        IPassTypeModelsMapper mapper
        ) : IKafkaMessageHandler<string, PTCRequestMessageRequestModel>
    {
        private const string infoMsg = "PassType calculation for (IssoId = {1}, Check point number = {2}) started";
        private const string errorMsg = "Failed PassType calculation for (IssoId = {1}, Check point number = {2})";
        private const string producerErrorMsg = "Message producer failed to send message";

        public async Task Handle(PTCRequestMessageRequestModel dto, MessageContext<string, PTCRequestMessageRequestModel> context)
        {
            PTCResultMessage responseContent = null;
            PTCRequestMessage message = null;
            try
            {
                message = mapper.FromDTO(dto);
                logger.LogInformation(string.Format(infoMsg, message.IssoId, message.CPNumber));
                responseContent = await ptcProcessor.CalculatePassType(message);
                await messageProducer.Produce(responseContent.GetBrokerId, mapper.ToDTO(responseContent));
            }
            catch (Exception e)
            {
                logger.LogError(string.Format(errorMsg, message?.IssoId, message?.CPNumber));
                if (responseContent != null && responseContent.IssoId > 0 && responseContent.CPNumber > 0)
                {
                    try
                    {
                        await messageProducer.Produce(responseContent.GetBrokerId, mapper.ToDTO(responseContent));
                    }
                    catch {
                        logger.LogError(e, producerErrorMsg);
                    }
                }
            }
        }
    }
}
