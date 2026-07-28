using System;
using System.Linq;
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
        ICanWork<StrainAnalysisParameters, StrainAnalysisResult> strainAnalyser, 
        ILogger<StrainAnalysisMessageHandler> logger,
        IKafkaProducer<string, AnalyseStrainCalculationResponse> messageProducer
        ) : IKafkaMessageHandler<string, StrainAnalysisCalculationRequest>
    {
        private const string producerErrorMsg = "Message producer failed to send message";
        private const string brokerClassNameStr = "strain-analysis";
        private const string strainCompareStr = "strain-compare";

        public async Task Handle(
            StrainAnalysisCalculationRequest dto, 
            MessageContext<string, StrainAnalysisCalculationRequest> context)
        {
            var data = dto.Adapt<StrainAnalysisParameters>();
            var key = context.ConsumeResults.Any(x => x.Message.Key.Equals(strainCompareStr)) ? strainCompareStr : brokerClassNameStr;
            try
            {
                var responseContent = await strainAnalyser.Run(data, new System.Threading.CancellationToken());
                await messageProducer.Produce(key, responseContent.Adapt<AnalyseStrainCalculationResponse>());
            }
            catch (Exception ex)
            {
                logger.LogError(producerErrorMsg, ex);
            }
        }
    }
}
