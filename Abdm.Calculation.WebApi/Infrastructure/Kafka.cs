using Abdm.Calculation.WebApi.Handlers;
using Abdm.Calculation.WebApi.Infrastructure.Messaging;
using Abdm.Calculation.WebApi.RequestModels;
using Abdm.Calculation.WebApi.ResponseModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Abdm.Calculation.Infrastructure
{
    public static class Kafka
    {
        public static void AddKafka(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddKafkaConsumer<string, PassTypeCalculationRequest, PassTypeCalculationMessageHandler>(consumer =>
            {
                configuration.GetSection("InternalCalculationMessageConsumer").Bind(consumer);
                consumer.ConsumersCount = configuration.GetValue<int>("ConsumersCount", 1);
            });

            services.AddKafkaProducer<string, PassTypeCalculationResponse>(producer =>
            {
                configuration.GetSection("InternalCalculationMessageProducer").Bind(producer);
            });

            services.AddKafkaConsumer<string, StrainAnalysisCalculationRequest, StrainAnalysisMessageHandler>(consumer =>
            {
                configuration.GetSection("StrainAnalysisMessageConsumer").Bind(consumer);
                consumer.ConsumersCount = configuration.GetValue<int>("ConsumersCount", 1);
            });

            services.AddKafkaProducer<string, AnalyseStrainCalculationResponse>(producer =>
            {
                configuration.GetSection("StrainAnalysisMessageProducer").Bind(producer);
            });
        }
    }
}
