namespace DataCollector.Messaging.Core;

public interface IBrokerModel : IDisposable
{
    Task SendMessageAsync(byte[] data, Uri endpoint);

    Task SubscribeMessageAsync(Func<MessageCallbackData, Task> callback, Uri endpoint);
}