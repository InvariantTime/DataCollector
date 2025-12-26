namespace DataCollector.Terminal.App.Services;

public interface IBackgroundService
{
    Task ExecuteAsync(CancellationToken cancellation);

    Task CloseAsync();
}
