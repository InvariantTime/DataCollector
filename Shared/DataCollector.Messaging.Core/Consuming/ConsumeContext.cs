namespace DataCollector.Messaging.Core.Consuming;

public class ConsumeContext<T> : IMessageBroker where T : class
{
    private readonly IMessageBroker _broker;

    public ConsumeContext(IMessageBroker broker)
    {
        _broker = broker;
    }

    public Task PublishAsync<TMessage>(TMessage context, string path) where TMessage : class
    {
        return _broker.PublishAsync(context, path);
    }

    public Task<IDisposable> SubscribeAsync<TMessage>(IMessageConsumer<TMessage> handler, string path) 
        where TMessage : class
    {
        return _broker.SubscribeAsync(handler, path);
    }
}
