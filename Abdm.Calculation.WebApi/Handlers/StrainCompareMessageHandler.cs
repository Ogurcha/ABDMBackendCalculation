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
    public class StrainCompareMessageHandler(
        ICanWork<StrainAnalysisParameters, StrainAnalysisResult> strainAnalyser,
        ILogger<StrainCompareMessageHandler> logger,
        IKafkaProducer<string, CompareStrainCalculationResponse> messageProducer
        ) : IKafkaMessageHandler<string, StrainCompareCalculationRequest>
    {
        private const string producerErrorMsg = "Message producer failed to send message";
        private const string brokerClassNameStr = "strain-compare";

        public async Task Handle(
            StrainCompareCalculationRequest dto,
            MessageContext<string, StrainCompareCalculationRequest> context)
        {
            var data = dto.Adapt<StrainAnalysisParameters>();
            try
            {
                var responseContent = await strainAnalyser.Run(data, new System.Threading.CancellationToken());
                await messageProducer.Produce(brokerClassNameStr, responseContent.Adapt<CompareStrainCalculationResponse>());
            }
            catch (Exception ex)
            {
                logger.LogError(producerErrorMsg, ex);
            }
        }
    }

    public class CompareStrainCalculationResponse : AnalyseStrainCalculationResponse
    {

    }

    public class StrainCompareCalculationRequest : StrainAnalysisCalculationRequest
    {

    }
}
