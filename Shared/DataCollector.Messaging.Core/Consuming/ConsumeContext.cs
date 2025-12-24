namespace DataCollector.Messaging.Core.Consuming;

public class ConsumeContext<T> : IMessageBroker where T : class
{
    private readonly IMessageBroker _broker;

    public T Message { get; }

    public string Route { get; }

    public ConsumeContext(IMessageBroker broker, T message, string route)
    {
        _broker = broker;
        Message = message;
        Route = route;
    }

    public Task PublishAsync<TMessage>(TMessage context) where TMessage : class
    {
        return _broker.PublishAsync(context);
    }

    public Task<IDisposable> SubscribeAsync<TMessage>(IMessageConsumer<TMessage> handler) 
        where TMessage : class
    {
        return _broker.SubscribeAsync(handler);
    }

    public Task<IDisposable> SubscribeAsync(IMessageConsumer handler)
    {
        return _broker.SubscribeAsync(handler);
    }

    public Task PublishAsync<TMessage>(TMessage context, Uri uri) where TMessage : class
    {
        return _broker.PublishAsync(context, uri);
    }

    public Task<IDisposable> SubscribeAsync<TMessage>(IMessageConsumer<TMessage> handler, Uri uri)
        where TMessage : class
    {
        return _broker.SubscribeAsync(handler, uri);
    }
}
