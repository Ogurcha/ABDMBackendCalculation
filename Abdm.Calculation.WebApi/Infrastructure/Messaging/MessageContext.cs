using System.Collections.Generic;
using Confluent.Kafka;

namespace Abdm.Calculation.WebApi.Infrastructure.Messaging
{
    /// <summary>
    /// Контекст обработки сообщения: даёт хендлеру доступ к исходным "сырым" результатам
    /// консьюмирования (метаданные партиции/оффсета, ключ и т.п.), из которых было получено
    /// десериализованное значение, переданное в <see cref="IKafkaMessageHandler{TKey, TValue}.Handle"/>.
    /// </summary>
    public class MessageContext<TKey, TValue>
    {
        public MessageContext(IReadOnlyList<ConsumeResult<TKey, TValue>> consumeResults)
        {
            ConsumeResults = consumeResults;
        }

        public IReadOnlyList<ConsumeResult<TKey, TValue>> ConsumeResults { get; }

        public ConsumeResult<TKey, TValue> ConsumeResult => ConsumeResults[0];
    }
}
