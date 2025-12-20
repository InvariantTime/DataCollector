
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
        var callback = _factory.CreateCallback(consumer);
        return await InternalSubscribeAsync(typeof(T), consumer, callback);
    }

    public async Task<IDisposable> SubscribeAsync(IMessageConsumer consumer)
    {
        var callback = _factory.CreateCallback(consumer);
        return await InternalSubscribeAsync(consumer.MessageType, consumer, callback);
    }

    private async Task<IDisposable> InternalSubscribeAsync(Type messageType, object consumer, ConsumerCallback callback)
    {
        await ReconnectIfNeed();// TODO: Reconnect policy

        var model = await _connection.CreateModelAsync();
        var options = _options.GetOrAdd(messageType, MessageOptions.CreateDefault);

        Func<MessageCallbackData, Task> nativeCallback = (data) =>
        {
            return callback.Invoke(data, this);
        };

        await model.SubscribeMessageAsync(nativeCallback, options.Endpoint);

        var holder = new ConsumerHolder(model, consumer, messageType, options.Endpoint, _holders);
        _holders.TryAdd(messageType, holder);

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
