
using DataCollector.Messaging.Core;
using DataCollector.Messaging.DI;

namespace DataCollector.Server.API;

public class BrokerStartupService : IHostedService
{
    private readonly MessageBrokerService _service;
    private readonly ILogger<BrokerStartupService> _logger;

    public BrokerStartupService(MessageBrokerService service, ILogger<BrokerStartupService> logger)
    {
        _service = service;
        _logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await _service.StartAsync();

        _logger.LogInformation("Message broker started!");
        _logger.LogInformation("Consumers count: {0}", _service.ConsumersCount);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
