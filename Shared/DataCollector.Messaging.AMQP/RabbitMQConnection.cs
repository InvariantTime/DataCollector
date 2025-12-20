using DataCollector.Messaging.Core;
using RabbitMQ.Client;

namespace DataCollector.Messaging.AMQP;

public class RabbitMQConnection : IBrokerConnection
{
    private readonly ConnectionFactory _factory;

    private IConnection? _currentConnection;

    public bool IsConnected => _currentConnection != null && _currentConnection.IsOpen == true;

    public RabbitMQConnection(RabbitMQOptions options)
    {
        _factory = new ConnectionFactory()
        {
            HostName = options.Host,
            Port = options.Port,
            VirtualHost = options.VirtualHost,
            UserName = options.User,
            Password = options.Password,
        };
    }

    public async Task<IBrokerModel> CreateModelAsync()
    {
        if (IsConnected == false)
            throw new InvalidOperationException("Connection is closed");

        var channel = await _currentConnection!.CreateChannelAsync();

        return new RabbitMQChannel(channel);
    }

    public async Task<bool> ReconnectAsync()
    {
        try
        {
            _currentConnection = await _factory.CreateConnectionAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }
}
