using RabbitMQ.Client;

namespace DataCollector.Shared.Events.RabbitMQ;

public class DefaultRabbitMQConnection : IRabbitMQConnection
{
    private readonly IConnectionFactory _factory;
    private IConnection? _connection;

    public bool IsConnected => _connection != null && _connection.IsOpen == true;

    public DefaultRabbitMQConnection(IConnectionFactory factory)
    {
        _factory = factory;
    }

    public async Task<IChannel> CreateChannelAsync()
    {
        bool isConnected = await ReconnectAsync();

        if (isConnected == false)
            return null!;//TODO: Result

        return await _connection!.CreateChannelAsync();
    }

    public async Task<bool> ReconnectAsync()
    {
        if (IsConnected == true)
            return true;

        var connection = await _factory.CreateConnectionAsync();//TODO: reconnect policy

        if (connection == null)
            return false;

        _connection = connection;

        return true;
    }
}
