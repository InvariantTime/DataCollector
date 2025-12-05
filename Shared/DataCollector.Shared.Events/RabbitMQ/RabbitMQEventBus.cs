using System.Collections.Concurrent;

namespace DataCollector.Shared.Events.RabbitMQ;

public class RabbitMQEventBus : IEventBus
{
    private readonly IRabbitMQConnection _connection;

    public RabbitMQEventBus(IRabbitMQConnection connection)
    {
        _connection = connection;
    }

    public async Task<bool> PublishAsync(EventDescription @event)
    {
        var channel = await _connection.CreateChannelAsync();

        if (channel == null)
            return false;


        return true;
    }

    public Task<IDisposable?> SubscribeAsync<T>(IEventHandler<T> handler) where T : EventDescription
    {
        

        return null!;
    }

    private sealed class SubscriptionNode : IDisposable
    {
        public SubscriptionNode()
        {

        }

        public void Dispose()
        {

        }
    }
}
