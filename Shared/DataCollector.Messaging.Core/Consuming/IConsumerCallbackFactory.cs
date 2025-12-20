namespace DataCollector.Messaging.Core.Consuming;

public delegate Task ConsumerCallback(MessageCallbackData data, IMessageBroker broker);

public interface IConsumerCallbackFactory
{
    ConsumerCallback CreateCallback(IMessageConsumer consumer);

    ConsumerCallback CreateCallback<T>(IMessageConsumer<T> consumer) where T : class;
}