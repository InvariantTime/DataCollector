namespace DataCollector.Messaging.Core.Consuming;

public interface IMessageConsumer<T> : IMessageConsumer where T : class
{
    Type IMessageConsumer.MessageType => typeof(T);

    Task ConsumeAsync(ConsumeContext<T> context);
}

public interface IMessageConsumer
{
    Type MessageType { get; }
}