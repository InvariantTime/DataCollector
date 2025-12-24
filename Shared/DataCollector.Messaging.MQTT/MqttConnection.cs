using DataCollector.Messaging.Core;
using MQTTnet;

namespace DataCollector.Messaging.MQTT;

public class MqttConnection : IBrokerConnection
{
    private readonly IMqttClient _client;
    private readonly MqttClientOptions _options;

    public bool IsConnected => _client.IsConnected;

    public MqttConnection(MqttOptions options)
    {
        var factory = new MqttClientFactory();

        _client = factory.CreateMqttClient();

        _options = new MqttClientOptionsBuilder()
            .WithTcpServer(options.Host, options.Port)
            .WithCredentials(options.User, options.Password)
            .Build();
    }

    public Task<IBrokerModel> CreateModelAsync()
    {
        return Task.FromResult<IBrokerModel>(new MQTTModel(_client));
    }

    public async Task<bool> ReconnectAsync()
    {
        var result = await _client.ConnectAsync(_options);
        return _client.IsConnected == true;
    }
}
