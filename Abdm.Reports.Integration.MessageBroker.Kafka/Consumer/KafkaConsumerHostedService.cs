using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Abdm.Integration.MessageBroker.Kafka.Consumer;

internal class KafkaConsumerHostedService : IHostedService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<KafkaConsumerHostedService> _logger;

    private IKafkaConsumer[] _consumers = Array.Empty<IKafkaConsumer>();

    public KafkaConsumerHostedService(IServiceProvider serviceProvider,
        ILogger<KafkaConsumerHostedService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        try
        {
            _consumers = _serviceProvider.GetServices<IKafkaConsumer>().ToArray();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Exception while activating kafka consumer {Message}", e.Message);
            throw;
        }

        foreach (var kafkaConsumer in _consumers)
        {
            _ = Task.Factory.StartNew(async () =>
            {
                try
                {
                    await kafkaConsumer.Start();
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Unhandled exception in kafka consumer");
                    throw;
                }

            }, TaskCreationOptions.LongRunning);
        }

        return Task.CompletedTask;
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        try
        {
            var consumers = _consumers;

            _logger.LogInformation("Stopping {ConsumersCount} kafka consumers",
                consumers.Length);

            await Task.WhenAll(consumers.Select(kc => kc.Stop()))
                .WaitAsync(cancellationToken);

            _logger.LogInformation("Kafka consumers stopped");
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Exception while stopping kafka consumers");
        }
    }
}
