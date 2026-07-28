using Confluent.Kafka;

namespace Abdm.Calculation.WebApi.Infrastructure.Messaging
{
    /// <summary>
    /// Настройки консьюмера Kafka, привязываемые из конфигурации приложения.
    /// </summary>
    public class KafkaConsumerSettings
    {
        public string BootstrapServers { get; set; } = string.Empty;

        public string Topic { get; set; } = string.Empty;

        public string ConsumerGroup { get; set; } = string.Empty;

        /// <summary>
        /// Максимальное число сообщений, обрабатываемых одновременно (степень параллелизма расчётов).
        /// Не равно числу IConsumer: клиент Confluent.Kafka не потокобезопасен, поэтому на топик
        /// поднимается один консьюмер, а Handle/Run выполняются параллельно до этого лимита.
        /// </summary>
        public int ConsumersCount { get; set; } = 1;

        public AutoOffsetReset AutoOffsetReset { get; set; } = AutoOffsetReset.Earliest;

        /// <summary>
        /// Отключено по умолчанию: оффсет коммитится вручную после успешной обработки сообщения хендлером,
        /// что даёт гарантию доставки "at least once".
        /// </summary>
        public bool EnableAutoCommit { get; set; } = false;
    }
}
