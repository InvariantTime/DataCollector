
namespace DataCollector.Terminal.App.Commands;

public partial class AsyncCommand : AsyncCommandBase
{
    private readonly Func<object?, CancellationToken, Task> _execution;
    private readonly Func<object?, bool>? _canExecute;

    public AsyncCommand(Func<object?, CancellationToken, Task> execution, Func<object?, bool>? canExecute)
    {
        _execution = execution ?? throw new ArgumentNullException();
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
        return _execution.Invoke(arg, cancellation);
    }

    public static AsyncCommand Create(Func<Task> execution)
    {
        Task Exec(object? _, CancellationToken __) => execution.Invoke();

        return new AsyncCommand(Exec, null);
    }

    public static AsyncCommand Create(Func<CancellationToken, Task> execution)
    {
        Task Exec(object? _, CancellationToken cancel) => execution.Invoke(cancel);

        return new AsyncCommand(Exec, null);
    }

    public static AsyncCommand Create(Func<Task> execution, Func<bool> canExecute)
    {
        Task Exec(object? _, CancellationToken __) => execution.Invoke();

        bool CanExecute(object? _) => canExecute.Invoke();

        return new AsyncCommand(Exec, CanExecute);
    }

    public static AsyncCommand Create(Func<CancellationToken, Task> execution, Func<bool> canExecute)
    {
        Task Exec(object? _, CancellationToken cancel) => execution.Invoke(cancel);

        bool CanExecute(object? _) => canExecute.Invoke();

        return new AsyncCommand(Exec, CanExecute);
    }

    public static AsyncCommand<T> Create<T>(Func<T, Task> execution)
    {
        Task Exec(T value, CancellationToken _) => execution.Invoke(value);

        return new AsyncCommand<T>(Exec, null);
    }

    public static AsyncCommand<T> Create<T>(Func<T, CancellationToken, Task> execution)
    {
        Task Exec(T value, CancellationToken cancel) => execution.Invoke(value, cancel);

        return new AsyncCommand<T>(Exec, null);
    }

    public static AsyncCommand<T> Create<T>(Func<T, Task> execution, Func<bool> canExecute)
    {
        Task Exec(T value, CancellationToken _) => execution.Invoke(value);

        bool CanExecute(object? _) => canExecute.Invoke();

        return new AsyncCommand<T>(Exec, CanExecute);
    }

    public static AsyncCommand<T> Create<T>(Func<T, CancellationToken, Task> execution, Func<bool> canExecute)
    {
        Task Exec(T value, CancellationToken cancel) => execution.Invoke(value, cancel);

        bool CanExecute(object? _) => canExecute.Invoke();

        return new AsyncCommand<T>(Exec, CanExecute);
    }
}
