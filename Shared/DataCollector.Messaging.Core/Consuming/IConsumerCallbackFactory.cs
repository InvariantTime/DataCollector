namespace DataCollector.Messaging.Core.Consuming;

public interface IConsumerCallbackFactory
{
    Action<MessageCallbackData> CreateCallback(object consumer);
}