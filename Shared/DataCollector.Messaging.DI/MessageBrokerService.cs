using DataCollector.Messaging.Core;
using DataCollector.Messaging.Core.Consuming;
using System.Collections.Immutable;

namespace DataCollector.Messaging.DI;

public class MessageBrokerService : IMessageBroker
{
    private readonly IMessageBroker _broker;
    private readonly ImmutableArray<IMessageConsumer> _baseConsumers;

    public bool IsStarted { get; private set; } = false;

    public int ConsumersCount => _baseConsumers.Length;

    public MessageBrokerService(IMessageBroker broker, IEnumerable<IMessageConsumer> consumers)
    {
        _broker = broker;
        _baseConsumers = consumers.ToImmutableArray();
    }

    public async Task StartAsync()
    {
        if (IsStarted == true)
            throw new InvalidOperationException("Broker service is already started");

        foreach (var consumer in _baseConsumers)
            await _broker.SubscribeAsync(consumer);
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
}
