using System;
using System.Threading;
using System.Threading.Tasks;
using Abdm.Calculation.BLL.Interfaces;
using Abdm.Calculation.BLL.Models.DataTransfer;
using Abdm.Calculation.WebApi.Infrastructure.Messaging;
using Abdm.Calculation.WebApi.RequestModels;
using Abdm.Calculation.WebApi.ResponseModels;
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
        private const string brokerClassNameStr = "load-calculation-group";

        public async Task Handle(
            PassTypeCalculationRequest dto, 
            MessageContext<string, PassTypeCalculationRequest> context,
            CancellationToken cancellationToken)
        {
            var data = dto.Adapt<PassTypeCalculationParameters>();
            try
            {
                var responseContent = await passTypeService.Run(data, cancellationToken);
                await messageProducer.Produce(brokerClassNameStr, responseContent.Adapt<PassTypeCalculationResponse>(), cancellationToken);
            }
            catch (Exception ex)
            {
                logger.LogError(producerErrorMsg, ex);
            }
        }
    }
}
