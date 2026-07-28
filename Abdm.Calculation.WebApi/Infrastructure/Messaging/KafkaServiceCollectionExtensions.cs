using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Abdm.Calculation.WebApi.Infrastructure.Messaging
{
    public static class KafkaServiceCollectionExtensions
    {
        /// <summary>
        /// Регистрирует консьюмер Kafka для топика/группы, описанных в <paramref name="configure"/>,
        /// и хендлер <typeparamref name="THandler"/>, вызываемый на каждое полученное сообщение.
        /// Если <see cref="KafkaConsumerSettings.ConsumersCount"/> больше 1, регистрирует несколько
        /// независимых hosted service-инстансов консьюмера (каждый в своём потоке) в рамках одной
        /// consumer group, что даёт реальную параллельную обработку партиций топика.
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

            var consumersCount = Math.Max(1, settings.ConsumersCount);

            services.AddScoped<THandler>();

            for (var workerId = 0; workerId < consumersCount; workerId++)
            {
                var currentWorkerId = workerId;
                services.AddSingleton<IHostedService>(sp => ActivatorUtilities.CreateInstance<KafkaConsumerWorker<TKey, TValue, THandler>>(
                    sp, settings, currentWorkerId));
            }

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
