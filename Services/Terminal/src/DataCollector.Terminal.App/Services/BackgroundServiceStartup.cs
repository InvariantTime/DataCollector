using System.Collections.Immutable;

namespace DataCollector.Terminal.App.Services;

public class BackgroundServiceStartup
{
    private readonly ImmutableArray<IBackgroundService> _backgroundServices;
    private readonly List<ServiceHolder> _holders;

    public BackgroundServiceStartup(IEnumerable<IBackgroundService> services)
    {
        _backgroundServices = services.ToImmutableArray();
        _holders = new List<ServiceHolder>();
    }

    public Task StartAsync()
    {
        foreach (var service in _backgroundServices)
        {
            var cts = new CancellationTokenSource();
            var task = Task.Run(() =>
            {
                return service.ExecuteAsync(cts.Token);
            }, cts.Token);

            _holders.Add(new ServiceHolder(task, service, cts));
        }

        return Task.CompletedTask;
    }

    public async Task StopAsync()
    {
        foreach (var holder in _holders)
        {
            try
            {
                holder.Cancellation.Cancel();
                await holder.ExecutionTask;
            }
            catch (OperationCanceledException)
            {
            }
            finally
            {
                await holder.Service.CloseAsync();
            }
        }
    }

    private record ServiceHolder(Task ExecutionTask, IBackgroundService Service, CancellationTokenSource Cancellation);
}
