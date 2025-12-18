namespace DataCollector.Messaging.Core.Consuming;

public interface IConsumerCallbackFactory
{
    Func<MessageCallbackData, Task> CreateCallback(object consumer);
}