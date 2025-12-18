namespace DataCollector.Messaging.Core;

public interface IModel : IDisposable
{
    Task SendMessageAsync(byte[] data, MessageEndpoint endpoint);

    Task SubscribeMessageAsync(Action<MessageCallbackData> callback, MessageEndpoint endpoint);
}