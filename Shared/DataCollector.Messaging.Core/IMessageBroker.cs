using DataCollector.Messaging.Core.Consuming;

namespace DataCollector.Messaging.Core;

public interface IMessageBroker
{
    Task PublishAsync<T>(T message) where T : class;

    Task PublishAsync<T>(T message, Uri uri) where T : class;

    Task<IDisposable> SubscribeAsync<T>(IMessageConsumer<T> consumer) where T : class;

    Task<IDisposable> SubscribeAsync(IMessageConsumer consumer);

    Task<IDisposable> SubscribeAsync<T>(IMessageConsumer<T> consumer, Uri uri) where T : class;
}