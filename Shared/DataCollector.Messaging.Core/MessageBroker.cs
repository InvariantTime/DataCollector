
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

    public Task PublishAsync<T>(T message) where T : class
    {
        var options = _options.GetOrAdd(typeof(T), MessageOptions.CreateDefault);
        return PublishAsync(message, options.Endpoint);
    }

    public async Task PublishAsync<T>(T message, Uri uri) where T : class
    {
        await ReconnectIfNeed();// TODO: Reconnect policy

        using var model = await _connection.CreateModelAsync();

        var json = JsonSerializer.Serialize(message); //TODO: different types and options of serialization
        var data = Encoding.UTF8.GetBytes(json) ?? [];//TODO: handle empty message as exception ?

        await model.SendMessageAsync(data, uri);
    }

    public async Task<IDisposable> SubscribeAsync<T>(IMessageConsumer<T> consumer) where T : class
    {
        var options = _options.GetOrAdd(typeof(T), MessageOptions.CreateDefault);
        return await SubscribeAsync(consumer, options.Endpoint);
    }

    public async Task<IDisposable> SubscribeAsync<T>(IMessageConsumer<T> consumer, Uri uri) where T : class
    {
        var callback = _factory.CreateCallback(consumer);
        return await InternalSubscribeAsync(typeof(T), consumer, callback, uri);
    }

    public async Task<IDisposable> SubscribeAsync(IMessageConsumer consumer)
    {
        var callback = _factory.CreateCallback(consumer);
        var options = _options.GetOrAdd(consumer.MessageType, MessageOptions.CreateDefault);
        return await InternalSubscribeAsync(consumer.MessageType, consumer, callback, options.Endpoint);
    }

    private async Task<IDisposable> InternalSubscribeAsync(Type messageType, object consumer, ConsumerCallback callback, Uri endpoint)
    {
        await ReconnectIfNeed();// TODO: Reconnect policy

        var model = await _connection.CreateModelAsync();

        Func<MessageCallbackData, Task> nativeCallback = (data) =>
        {
            return callback.Invoke(data, this);
        };

        await model.SubscribeMessageAsync(nativeCallback, endpoint);

        var holder = new ConsumerHolder(model, consumer, messageType, endpoint, _holders);
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
