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
    public class StrainAnalysisMessageHandler(
        ICanWork<PassTypeCalculationParameters, StrainAnalysisResult> strainAnalyser, 
        ILogger<StrainAnalysisMessageHandler> logger,
        IKafkaProducer<string, AnalyseStrainCalculationResponse> messageProducer
        ) : IKafkaMessageHandler<string, StrainAnalysisCalculationRequest>
    {
        private const string producerErrorMsg = "Message producer failed to send message";
        private const string brokerClassNameStr = "strain-analysis";

        public async Task Handle(
            StrainAnalysisCalculationRequest dto, 
            MessageContext<string, StrainAnalysisCalculationRequest> context)
        {
            var data = dto.Adapt<PassTypeCalculationParameters>();
            try
            {
                var responseContent = await strainAnalyser.Run(data, new System.Threading.CancellationToken());
                await messageProducer.Produce(brokerClassNameStr, responseContent.Adapt<AnalyseStrainCalculationResponse>());
            }
            catch (Exception ex)
            {
                logger.LogError(producerErrorMsg, ex);
            }
        }
    }
}
