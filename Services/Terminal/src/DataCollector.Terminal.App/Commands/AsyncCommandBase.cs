using System.Windows.Input;

namespace DataCollector.Terminal.App.Commands;

public abstract partial class AsyncCommandBase : IAsyncCommand
{
    private readonly WeakEventManager _eventManager;
    private AsyncCommandTask? _task;

    public bool IsExecuting => _task != null && _task.ExecutionTask.Status == TaskStatus.Running;

    public event EventHandler? CanExecuteChanged
    {
        add => _eventManager.AddEventHandler(value);

        remove => _eventManager.RemoveEventHandler(value);
    }

    public AsyncCommandBase()
    {
        _eventManager = new WeakEventManager();
    }

    public abstract Task ExecuteAsync(object? arg, CancellationToken cancellation);

    public abstract bool CanExecute(object? parameter);

    public async Task CancelAsync()
    {
        if (_task == null)
            return;

        _task.CTS.Cancel();

        try
        {
            await _task.ExecutionTask;
        }
        catch (OperationCanceledException)
        {
        }
    }

    public void ChangeCanExecute()
    {
        _eventManager.HandleEvent(this, EventArgs.Empty, nameof(CanExecuteChanged));
    }

    void ICommand.Execute(object? parameter)
    {
        var cts = new CancellationTokenSource();
        var task = ExecuteAsync(parameter, cts.Token);

        _task = new AsyncCommandTask(task, cts);
    }
}
