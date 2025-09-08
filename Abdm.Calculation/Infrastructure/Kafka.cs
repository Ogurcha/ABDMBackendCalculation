using Abdm.Calculation.ColumnCalculation;
using Abdm.Calculation.WebApi.DTO;
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
            services.AddKafkaConsumer<string, PTCRequestMessageDTO, PTCMessageHandler>(consumer =>
            {
                consumer.Configuration.LoadFromConfiguration("InternalCalculationMessageConsumer");
                consumer.UseJsonMessageDeserializer();
                consumer.ConsumersCount = 1;
            });

            services.AddKafkaProducer<string, PTCResultMessageDTO>(producer =>
            {
                producer.Configuration.LoadFromConfiguration("InternalCalculationMessageProducer");
                producer.UseJsonMessageSerializer();
            });
        }
    }
}
