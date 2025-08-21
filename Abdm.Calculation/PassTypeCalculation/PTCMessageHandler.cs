using System.Threading.Tasks;
using Abdm.Calculation.PassTypeCalculation.DTO;
using Abdm.Integration.MessageBroker.Kafka.Consumer;

namespace Abdm.Calculation.ColumnCalculation
{
    public class PTCMessageHandler(IPTCProcessor cCProcessor) : IKafkaMessageHandler<string, PTCRequestMessage>
    {
        public async Task Handle(PTCRequestMessage message, MessageContext<string, PTCRequestMessage> context)
        {
            await cCProcessor.Process(message);




        }
    }
}
