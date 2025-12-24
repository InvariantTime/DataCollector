using DataCollector.Messaging.Core;
using MQTTnet;
using MQTTnet.Protocol;
using System.Buffers;

namespace DataCollector.Messaging.MQTT;

public class MQTTModel : IBrokerModel
{
    private readonly IMqttClient _client;
    private IDisposable? _consumerDisposable;

    public MQTTModel(IMqttClient client)
    {
        _client = client;
    }

    public Task SendMessageAsync(byte[] data, Uri endpoint)//TODO: parser for endpoint
    {
        var message = new MqttApplicationMessageBuilder()
            .WithTopic(endpoint.AbsolutePath)
            .WithPayload(data)
            .WithQualityOfServiceLevel(MqttQualityOfServiceLevel.AtLeastOnce);

        return _client.PublishAsync(message.Build());
    }

    public async Task SubscribeMessageAsync(Func<MessageCallbackData, Task> nativeCallback, Uri endpoint)//TODO: parser for endpoint
    {
        string topic = endpoint.AbsolutePath;
        await _client.SubscribeAsync(topic);

        Func<MqttApplicationMessageReceivedEventArgs, Task> handler = (args) =>
        {
            if (args.ApplicationMessage.Topic != topic)
                return Task.CompletedTask;

            var data = new MessageCallbackData
            {
                Payload = args.ApplicationMessage.Payload.ToArray(),
                Route = args.ApplicationMessage.Topic
            };

            return nativeCallback.Invoke(data);
        };

        _client.ApplicationMessageReceivedAsync += handler;
        _consumerDisposable = ConsumeHolder.Create(topic, handler, _client);
    }

    public void Dispose()
    {
        _consumerDisposable?.Dispose();
    }

    private readonly struct ConsumeHolder : IDisposable
    {
        private readonly string _topic;
        private readonly Func<MqttApplicationMessageReceivedEventArgs, Task> _handler;
        private readonly IMqttClient _client;

        public ConsumeHolder(string topic, Func<MqttApplicationMessageReceivedEventArgs, Task> handler, IMqttClient client)
        {
            _topic = topic;
            _handler = handler;
            _client = client;
        }

        public static ConsumeHolder Create(string topic, Func<MqttApplicationMessageReceivedEventArgs, Task> handler, IMqttClient client)
        {
            return new ConsumeHolder(topic, handler, client);
        }

        public void Dispose()
        {
            _client.UnsubscribeAsync(_topic).GetAwaiter().GetResult();
            _client.ApplicationMessageReceivedAsync -= _handler;
        }
    }
}
