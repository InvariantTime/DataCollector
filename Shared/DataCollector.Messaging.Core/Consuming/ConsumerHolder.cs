using System.Net;

namespace DataCollector.Messaging.Core.Consuming;

internal sealed class ConsumerHolder : IDisposable
{
    private readonly IDictionary<Type, ConsumerHolder> _container;

    public IBrokerModel Model { get; }

    public object Consumer { get; }

    public Type MessageType { get; }

    public Uri Endpoint { get; }

    public ConsumerHolder(IBrokerModel model, 
        object consumer, 
        Type messageType, 
        Uri endpoint, 
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
