using System.Threading;
using System.Threading.Tasks;

namespace Abdm.Calculation.WebApi.Infrastructure.Messaging
{
    public interface IKafkaProducer<TKey, TValue>
    {
        Task Produce(TKey key, TValue value, CancellationToken cancellationToken = default);
    }
}
