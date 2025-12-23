namespace DataCollector.Terminal.App.Forms;

public interface IWithParameter<T>
{
    Task InitializeParameterAsync(T parameter);
}

public interface IWithResult<T>
{
    Task<T> AwaitResultAsync(CancellationToken cancellation);
}