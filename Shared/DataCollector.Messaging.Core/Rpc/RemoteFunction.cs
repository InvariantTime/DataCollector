using DataCollector.Messaging.Core.Consuming;

namespace DataCollector.Messaging.Core.Rpc;

public class RemoteFunction<TArg, TResult> : IDisposable
    where TArg : class
    where TResult : class
{
    private readonly IMessageBroker _broker;

    private IDisposable? _consumerHolder;
    private TaskCompletionSource<TResult> _tcs;

    public RemoteFunction(IMessageBroker broker)
    {
        _tcs = new TaskCompletionSource<TResult>();
        _broker = broker;
    }

    public async Task<TResult> ExecuteRemoteAsync(TArg arg, CancellationToken cancellation)
    {
        if (_consumerHolder == null)
            _consumerHolder = await CreateConsumerAsync();

        await _broker.PublishAsync(arg);

        var data = await _tcs.Task.WaitAsync(cancellation);
        _tcs = new TaskCompletionSource<TResult>();

        return data;
    }

    public async Task<TResult> ExecuteRemoteAsync(TArg arg, Uri request, CancellationToken cancellation)
    {
        if (_consumerHolder == null)
            _consumerHolder = await CreateConsumerAsync();

        await _broker.PublishAsync(arg, request);

        var data = await _tcs.Task.WaitAsync(cancellation);
        _tcs = new TaskCompletionSource<TResult>();

        return data;
    }

    public void Dispose()
    {
        _consumerHolder?.Dispose();
    }

    private Task<IDisposable> CreateConsumerAsync()
    {
        var lambdaConsumer = new LambdaConsumer<TResult>(OnMessageRecived);
        return _broker.SubscribeAsync(lambdaConsumer);
    }

    private Task OnMessageRecived(ConsumeContext<TResult> context)
    {
        _tcs.SetResult(context.Message);
        return Task.CompletedTask;
    }
}