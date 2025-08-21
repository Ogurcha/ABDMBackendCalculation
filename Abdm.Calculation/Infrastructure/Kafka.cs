using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Abdm.Integration.MessageBroker.Kafka.Consumer.Extensions;
using Abdm.Integration.MessageBroker.Kafka.Producer.Extensions;
using Abdm.Integration.MessageBroker.Kafka.Serialization.Extensions;
using Abdm.Calculation.ColumnCalculation;
using Abdm.Calculation.PassTypeCalculation.DTO;

namespace Abdm.Calculation.Infrastructure
{
    public static class Kafka
    {
        public static void AddKafka(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddKafkaConsumer<string, PTCRequestMessage, PTCMessageHandler>(consumer =>
            {
                consumer.Configuration.LoadFromConfiguration("InternalCalculationMessageConsumer");
                consumer.UseJsonMessageDeserializer();
                consumer.ConsumersCount = 1;
            });

            services.AddKafkaProducer<string, PTCResultMessage>(producer =>
            {
                producer.Configuration.LoadFromConfiguration("InternalCalculationMessageProducer");
                producer.UseJsonMessageSerializer();
            });
        }
    }
}
