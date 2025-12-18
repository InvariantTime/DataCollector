namespace DataCollector.Messaging.Core.Consuming;

internal sealed class ConsumerHolder : IDisposable
{
    private readonly IDictionary<Type, ConsumerHolder> _container;

    public IModel Model { get; }

    public object Consumer { get; }

    public Type MessageType { get; }

    public MessageEndpoint Endpoint { get; }

    public ConsumerHolder(IModel model, 
        object consumer, 
        Type messageType, 
        MessageEndpoint endpoint, 
        IDictionary<Type, ConsumerHolder> container)
    {
        Model = model;
        Consumer = consumer;
        MessageType = messageType;
        Endpoint = endpoint;
        _container = container;
    }

    public void Dispose()
    {
        _container.Remove(MessageType, out _);
        Model.Dispose();
    }
}
