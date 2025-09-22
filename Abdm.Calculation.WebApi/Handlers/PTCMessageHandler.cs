using System;
using System.Threading.Tasks;
using Abdm.Calculation.WebApi.Mappers;
using Abdm.Calculation.WebApi.RequestModels;
using Abdm.Calculation.WebApi.ResponseModels;
using Kafka.Integration.MessageBroker.Consumer;
using Kafka.Integration.MessageBroker.Producer;
using Microsoft.Extensions.Logging;

namespace Abdm.Calculation.WebApi.Handlers
{

    /// <summary>
    /// Хендлер для сообщений из брокера.
    /// Рассчет условий пропуска
    /// </summary>
    public class PTCMessageHandler(
        IPassTypeService passTypeService, 
        ILogger<PTCMessageHandler> logger,
        IKafkaProducer<string, PTCResultMessageResponseModel> messageProducer,
        IPassTypeModelsMapper mapper
        ) : IKafkaMessageHandler<string, PTCRequestMessageRequestModel>
    {
        private const string producerErrorMsg = "Message producer failed to send message";
        private const string brokerClassNameStr = "class-calculated";

        public async Task Handle(
            PTCRequestMessageRequestModel dto, 
            MessageContext<string, PTCRequestMessageRequestModel> context)
        {
            var data = mapper.FromDTO(dto);
            try
            {
                var responseContent = await passTypeService.GetPassType(data);
                await messageProducer.Produce(brokerClassNameStr, mapper.ToDTO(responseContent));
            }
            catch (Exception ex)
            {
                logger.LogError(producerErrorMsg, ex);
            }
        }
    }
}
