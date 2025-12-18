namespace DataCollector.Messaging.Core;

public interface IBrokerConnection
{
    bool IsConnected { get; }

    Task<bool> ReconnectAsync();

    Task<IBrokerModel> CreateModelAsync();
}
