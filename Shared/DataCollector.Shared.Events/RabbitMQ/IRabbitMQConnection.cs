using RabbitMQ.Client;

namespace DataCollector.Shared.Events.RabbitMQ;

public interface IRabbitMQConnection
{
    bool IsConnected { get; }

    Task<bool> ReconnectAsync();

    Task<IChannel> CreateChannelAsync();
}
