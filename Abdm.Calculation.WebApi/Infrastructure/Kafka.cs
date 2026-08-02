using Abdm.Calculation.WebApi.Handlers;
using Abdm.Calculation.WebApi.RequestModels;
using Abdm.Calculation.WebApi.ResponseModels;
using Kafka.Integration.MessageBroker.Consumer.Extensions;
using Kafka.Integration.MessageBroker.Producer.Extensions;
using Kafka.Integration.MessageBroker.Serialization.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Abdm.Calculation.Infrastructure
{
    public static class Kafka
    {
        public static void AddKafka(this IServiceCollection services, IConfiguration configuration)
        {
#if !DEBUG
            services.AddKafkaConsumer<string, PassTypeCalculationRequest, PassTypeCalculationMessageHandler>(consumer =>
            {
                consumer.Configuration.LoadFromConfiguration("InternalCalculationMessageConsumer");
                consumer.UseJsonMessageDeserializer();
                consumer.ConsumersCount = configuration.GetValue<int>("ConsumersCount", 1);
            });

            services.AddKafkaProducer<string, PassTypeCalculationResponse>(producer =>
            {
                producer.Configuration.LoadFromConfiguration("InternalCalculationMessageProducer");
                producer.UseJsonMessageSerializer();
            });
#endif

            services.AddKafkaConsumer<string, StrainAnalysisCalculationRequest, StrainAnalysisMessageHandler>(consumer =>
            {
#if DEBUG
                consumer.Configuration.LoadFromConfiguration("StrainAnalysisMessageConsumerForDebuggin");
#else
                consumer.Configuration.LoadFromConfiguration("StrainAnalysisMessageConsumer");
#endif
                consumer.UseJsonMessageDeserializer();
                consumer.ConsumersCount = configuration.GetValue<int>("ConsumersCount", 1);
            });

            services.AddKafkaProducer<string, AnalyseStrainCalculationResponse>(producer =>
            {
                producer.Configuration.LoadFromConfiguration("StrainAnalysisMessageProducer");
                producer.UseJsonMessageSerializer();
            });
        }
    }
}
