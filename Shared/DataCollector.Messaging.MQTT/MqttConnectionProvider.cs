using DataCollector.Messaging.Core;

namespace DataCollector.Messaging.MQTT;

public class MqttConnectionProvider : IBrokerConnection
{
    private MqttConnection? _currentConnection;

    public bool IsConnected => _currentConnection != null && _currentConnection.IsConnected == true;

    public bool ApplyConnectionSettings(MqttOptions options)
    {
        try
        {
            _currentConnection = new MqttConnection(options);
            _currentConnection.ReconnectAsync().Wait();
            return true;
        }
        catch
        {
            return false;
        }
    }

    public Task<IBrokerModel> CreateModelAsync()
    {
        if (_currentConnection == null)
            throw new InvalidOperationException("Broker has not connection");

        return _currentConnection.CreateModelAsync();
    }

    public Task<bool> ReconnectAsync()
    {
        if (_currentConnection == null)
            throw new InvalidOperationException("Broker has not connection");

        return _currentConnection.ReconnectAsync();
    }
}
