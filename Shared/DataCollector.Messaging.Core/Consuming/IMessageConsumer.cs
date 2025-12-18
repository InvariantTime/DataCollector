namespace DataCollector.Messaging.Core.Consuming;

public interface IMessageConsumer<T> where T : class
{
    Task ConsumeAsync(ConsumeContext<T> context);
}
