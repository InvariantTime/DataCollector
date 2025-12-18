
using DataCollector.Messaging.Core.Consuming;
using System.Collections.Concurrent;
using System.Text;
using System.Text.Json;

namespace DataCollector.Messaging.Core;

public class MessageBroker : IMessageBroker
{
    private readonly IBrokerConnection _connection;
    private readonly IConsumerCallbackFactory _factory;
    private readonly ConcurrentDictionary<Type, MessageOptions> _options;
    private readonly ConcurrentDictionary<Type, ConsumerHolder> _holders;

    public MessageBroker(IBrokerConnection connection, IConsumerCallbackFactory factory, IEnumerable<MessageOptions> options)
    {
        _connection = connection;
        _options = new ConcurrentDictionary<Type, MessageOptions>(options.ToDictionary(x => x.MessageType));
        _holders = new ConcurrentDictionary<Type, ConsumerHolder>();
        _factory = factory;
    }

    public async Task PublishAsync<T>(T message) where T : class
    {
        await ReconnectIfNeed();// TODO: Reconnect policy

        using var model = await _connection.CreateModelAsync();
        var options = _options.GetOrAdd(typeof(T), MessageOptions.CreateDefault);

        var json = JsonSerializer.Serialize(message); //TODO: different types and options of serialization
        var data = Encoding.UTF8.GetBytes(json) ?? [];//TODO: handle empty message as exception ?

        await model.SendMessageAsync(data, options.Endpoint);
    }

    public async Task<IDisposable> SubscribeAsync<T>(IMessageConsumer<T> consumer) where T : class
    {
        await ReconnectIfNeed();// TODO: Reconnect policy

        var model = await _connection.CreateModelAsync();

        var options = _options.GetOrAdd(typeof(T), MessageOptions.CreateDefault);
        var callback = _factory.CreateCallback(consumer);

        await model.SubscribeMessageAsync(callback, options.Endpoint);

        var holder = new ConsumerHolder(model, consumer, typeof(T), options.Endpoint, _holders);
        _holders.TryAdd(typeof(T), holder);

        return holder;
    }

    private async Task ReconnectIfNeed()
    {
        if (_connection.IsConnected == false)
        {
            var reconnectionResult = await _connection.ReconnectAsync();

            if (reconnectionResult == false)
                throw new Exception("Unable to connect");
        }
    }
}
