namespace DataCollector.Shared.Events;

public interface IEventBus
{
    Task<bool> PublishAsync(EventDescription @event);//TODO: Result

    Task<IDisposable?> SubscribeAsync<T>(IEventHandler<T> handler) where T : EventDescription;
}