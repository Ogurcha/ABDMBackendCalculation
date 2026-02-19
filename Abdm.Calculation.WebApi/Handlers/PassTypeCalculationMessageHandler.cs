using System;
using System.Threading.Tasks;
using Abdm.Calculation.BLL.Interfaces;
using Abdm.Calculation.BLL.Models.DataTransfer;
using Abdm.Calculation.WebApi.RequestModels;
using Abdm.Calculation.WebApi.ResponseModels;
using Kafka.Integration.MessageBroker.Consumer;
using Kafka.Integration.MessageBroker.Producer;
using Mapster;
using Microsoft.Extensions.Logging;

namespace Abdm.Calculation.WebApi.Handlers
{

    /// <summary>
    /// Хендлер для сообщений из брокера.
    /// Рассчет условий пропуска
    /// </summary>
    public class PassTypeCalculationMessageHandler(
        ICanWork<PassTypeCalculationParameters, PassTypeCalculationResult> passTypeService, 
        ILogger<PassTypeCalculationMessageHandler> logger,
        IKafkaProducer<string, PassTypeCalculationResponse> messageProducer
        ) : IKafkaMessageHandler<string, PassTypeCalculationRequest>
    {
        private const string producerErrorMsg = "Message producer failed to send message";
        private const string brokerClassNameStr = "class-calculated";

        public async Task Handle(
            PassTypeCalculationRequest dto, 
            MessageContext<string, PassTypeCalculationRequest> context)
        {
            var data = dto.Adapt<PassTypeCalculationParameters>();
            try
            {
                var responseContent = await passTypeService.Run(data, new System.Threading.CancellationToken());
                await messageProducer.Produce(brokerClassNameStr, responseContent.Adapt<PassTypeCalculationResponse>());
            }
            catch (Exception ex)
            {
                logger.LogError(producerErrorMsg, ex);
            }
        }
    }
}
