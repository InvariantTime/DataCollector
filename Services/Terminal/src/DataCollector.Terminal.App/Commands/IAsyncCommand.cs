using System.Windows.Input;

namespace DataCollector.Terminal.App.Commands;

public interface IAsyncCommand : ICommand
{
    Task ExecuteAsync(object? arg, CancellationToken cancellation);

    Task CancelAsync();
}

public interface IAsyncCommand<T> : IAsyncCommand
{
    Task ExecuteAsync(T arg, CancellationToken cancellation);
}
