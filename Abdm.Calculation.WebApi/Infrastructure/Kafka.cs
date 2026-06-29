using Abdm.Calculation.WebApi.Handlers;
using Abdm.Calculation.WebApi.RequestModels;
using Abdm.Calculation.WebApi.ResponseModels;
using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Abdm.Calculation.Infrastructure
{
    public static class Kafka
    {
        public static void AddKafka(this IServiceCollection services, IConfiguration configuration)
        {
            var consumersCount = configuration.GetValue<int>("ConsumersCount", 1);

            var kafkaSection = configuration.GetSection("InternalCalculationMessageConsumer");
            services.AddSingleton<IConsumer<PassTypeCalculationRequest, PassTypeCalculationMessageHandler>>(sp =>
            {
                var config = new ProducerConfig
                {
                    BootstrapServers = kafkaSection["BootstrapServers"],
                    
                };
                return new ProducerBuilder<string, string>(config).Build();
            });




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

            services.AddKafkaConsumer<string, StrainAnalysisCalculationRequest, StrainAnalysisMessageHandler>(consumer =>
            {
                consumer.Configuration.LoadFromConfiguration("StrainAnalysisMessageConsumer");
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
