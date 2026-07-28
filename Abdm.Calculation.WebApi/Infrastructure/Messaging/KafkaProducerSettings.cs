namespace Abdm.Calculation.WebApi.Infrastructure.Messaging
{
    /// <summary>
    /// Настройки продюсера Kafka, привязываемые из конфигурации приложения.
    /// </summary>
    public class KafkaProducerSettings
    {
        public string BootstrapServers { get; set; } = string.Empty;

        public string Topic { get; set; } = string.Empty;
    }
}
