namespace DataCollector.Shared.Events;

public interface IEventHandler<T>
{
    void HandleEvent(T @event);
}
