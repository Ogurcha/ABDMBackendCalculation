using System.Threading.Tasks;
using Abdm.Calculation.Models;
using Abdm.Integration.MessageBroker.Kafka.Consumer;

namespace Abdm.Calculation.ColumnCalculation
{
    public class CCMessageHandler(ICCProcessor cCProcessor) : IKafkaMessageHandler<string, CCRequestMessage>
    {
        public async Task Handle(CCRequestMessage message, MessageContext<string, CCRequestMessage> context)
        {
            await cCProcessor.Process(message);
        }
    }
}
