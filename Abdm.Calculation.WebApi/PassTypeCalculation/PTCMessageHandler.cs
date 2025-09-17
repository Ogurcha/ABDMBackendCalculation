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
        private const string infoMsg = "PassType calculation for (IssoId = {0}, Check point number = {1}) started";
        private const string errorMsg = "Failed PassType calculation for (IssoId = {0}, Check point number = {1})";
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
                await messageProducer.Produce("class-calculated", mapper.ToDTO(responseContent));
            }
            catch (Exception e)
            {
                logger.LogError(string.Format(errorMsg, message?.IssoId, message?.CPNumber));
                logger.LogError(e.StackTrace);
                try
                {
                    var data = ptcProcessor.GetFailedResponse(mapper.FromDTO(dto));
                    await messageProducer.Produce("class-calculated", mapper.ToDTO(data));
                }
                catch
                {
                    logger.LogError(e, producerErrorMsg);
                    logger.LogError(e.StackTrace);
                }
            }
        }
    }
}
