using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Abdm.Calculation.WebApi.Infrastructure.Messaging
{
    public static class KafkaServiceCollectionExtensions
    {
        /// <summary>
        /// Регистрирует один Kafka-консьюмер для топика/группы из <paramref name="configure"/>
        /// и хендлер <typeparamref name="THandler"/>.
        /// <see cref="KafkaConsumerSettings.ConsumersCount"/> задаёт максимальную степень параллелизма
        /// обработки сообщений (сколько расчётов могут идти одновременно), а не число IConsumer.
        /// </summary>
        public static IServiceCollection AddKafkaConsumer<TKey, TValue, THandler>(
            this IServiceCollection services,
            Action<KafkaConsumerSettings> configure)
            where THandler : class, IKafkaMessageHandler<TKey, TValue>
        {
            var settings = new KafkaConsumerSettings();
            configure(settings);

            if (string.IsNullOrWhiteSpace(settings.BootstrapServers) || string.IsNullOrWhiteSpace(settings.Topic))
            {
                throw new InvalidOperationException(
                    $"Kafka consumer for {typeof(TValue).Name} is missing BootstrapServers/Topic configuration.");
            }

            services.AddScoped<THandler>();

            // Один IConsumer на регистрацию: клиент Confluent.Kafka не потокобезопасен.
            // Параллелизм = ConsumersCount внутри воркера (несколько Handle одновременно).
            services.AddSingleton<IHostedService>(sp =>
                ActivatorUtilities.CreateInstance<KafkaConsumerWorker<TKey, TValue, THandler>>(sp, settings, 0));

            return services;
        }

        /// <summary>
        /// Регистрирует продюсер Kafka как singleton поверх Confluent.Kafka.
        /// </summary>
        public static IServiceCollection AddKafkaProducer<TKey, TValue>(
            this IServiceCollection services,
            Action<KafkaProducerSettings> configure)
        {
            var settings = new KafkaProducerSettings();
            configure(settings);

            if (string.IsNullOrWhiteSpace(settings.BootstrapServers) || string.IsNullOrWhiteSpace(settings.Topic))
            {
                throw new InvalidOperationException(
                    $"Kafka producer for {typeof(TValue).Name} is missing BootstrapServers/Topic configuration.");
            }

            services.AddSingleton<IKafkaProducer<TKey, TValue>>(sp =>
                ActivatorUtilities.CreateInstance<KafkaProducer<TKey, TValue>>(sp, settings));

            return services;
        }
    }
}
