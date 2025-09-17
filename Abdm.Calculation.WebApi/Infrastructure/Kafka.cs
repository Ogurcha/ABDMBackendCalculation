using Abdm.Calculation.WebApi;
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
            services.AddKafkaConsumer<string, PTCRequestMessageRequestModel, PTCMessageHandler>(consumer =>
            {
                consumer.Configuration.LoadFromConfiguration("InternalCalculationMessageConsumer");
                consumer.UseJsonMessageDeserializer();
                consumer.ConsumersCount = 1;
            });

            services.AddKafkaProducer<string, PTCResultMessageResponseModel>(producer =>
            {
                producer.Configuration.LoadFromConfiguration("InternalCalculationMessageProducer");
                producer.UseJsonMessageSerializer();
            });
        }
    }
}
