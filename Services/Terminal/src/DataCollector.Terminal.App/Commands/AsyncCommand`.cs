
namespace DataCollector.Terminal.App.Commands;

public partial class AsyncCommand<T> : AsyncCommandBase, IAsyncCommand<T>
{
    private readonly Func<T, CancellationToken, Task> _execution;
    private readonly Func<object?, bool>? _canExecute;

    public AsyncCommand(Func<T, CancellationToken, Task> execution, Func<object?, bool>? canExecute)
    {
        _execution = execution;
        _canExecute = canExecute;
    }

    public override bool CanExecute(object? parameter)
    {
        if (_canExecute != null)
            return _canExecute.Invoke(parameter);

        return IsExecuting == false;
    }

    public override Task ExecuteAsync(object? arg, CancellationToken cancellation)
    {
        if (arg is not T t)
            throw new Exception($"Unable to cast command argument to: {typeof(T)}");

        return ExecuteAsync(t, cancellation);
    }

    public Task ExecuteAsync(T arg, CancellationToken cancellation)
    {
        return _execution.Invoke(arg, cancellation);
    }
}
