using System.Threading;
using System.Threading.Tasks;

namespace Abdm.Calculation.WebApi.Infrastructure.Messaging
{
    /// <summary>
    /// Обработчик десериализованного сообщения из Kafka.
    /// Регистрируется в DI как Scoped и разрешается заново для каждого сообщения.
    /// </summary>
    public interface IKafkaMessageHandler<TKey, TValue>
    {
        Task Handle(TValue message, MessageContext<TKey, TValue> context, CancellationToken cancellationToken);
    }
}
