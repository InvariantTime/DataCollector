namespace DataCollector.Terminal.App.Commands;

public class AsyncCommandTask
{
    public Task ExecutionTask { get; }

    public CancellationTokenSource CTS { get; }

    public AsyncCommandTask(Task execution, CancellationTokenSource cts)
    {
        ExecutionTask = execution;
        CTS = cts;
    }
}
