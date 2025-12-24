using DataCollector.Messaging.Core;
using DataCollector.Messaging.Core.Consuming;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Immutable;

namespace DataCollector.Messaging.DI;

public class MessageBrokerService : IMessageBroker
{
    private readonly IMessageBroker _broker;

    public bool IsStarted { get; private set; } = false;

    public IServiceScope ConsumersScope { get; }

    public MessageBrokerService(IMessageBroker broker, IServiceScope consumersScope)
    {
        _broker = broker;
        ConsumersScope = consumersScope;
    }

    public async Task<int> StartAsync()
    {
        if (IsStarted == true)
            throw new InvalidOperationException("Broker service is already started");

        var consumers = ConsumersScope.ServiceProvider.GetRequiredService<IEnumerable<IMessageConsumer>>();
        
        foreach (var consumer in consumers)
            await _broker.SubscribeAsync(consumer);

        return consumers.Count();
    }

    public Task PublishAsync<T>(T message) where T : class
    {
        return _broker.PublishAsync(message);
    }

    public Task<IDisposable> SubscribeAsync<T>(IMessageConsumer<T> consumer) where T : class
    {
        return _broker.SubscribeAsync(consumer);
    }

    public Task<IDisposable> SubscribeAsync(IMessageConsumer consumer)
    {
        return _broker.SubscribeAsync(consumer);
    }

    public Task PublishAsync<T>(T message, Uri uri) where T : class
    {
        return _broker.PublishAsync(message, uri);
    }

    public Task<IDisposable> SubscribeAsync<T>(IMessageConsumer<T> consumer, Uri uri) where T : class
    {
        return _broker.SubscribeAsync(consumer, uri);
    }
}
