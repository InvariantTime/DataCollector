using DataCollector.Messaging.Core.Consuming;

namespace DataCollector.Messaging.Core.Rpc;

public class RemoteFunction<TArg, TResult> : IDisposable
    where TArg : class
    where TResult : class
{
    private readonly IMessageBroker _broker;
    private readonly Uri? _responceUri;

    private IDisposable? _consumerHolder;
    private TaskCompletionSource<TResult> _tcs;

    public RemoteFunction(IMessageBroker broker, Uri? responceUri = null)
    {
        _tcs = new TaskCompletionSource<TResult>();
        _broker = broker;
        _responceUri = responceUri;
    }

    public async Task<TResult?> ExecuteRemoteAsync(TArg arg, CancellationToken cancellation)
    {
        if (_consumerHolder == null)
            _consumerHolder = await CreateConsumerAsync();

        await _broker.PublishAsync(arg);

        try
        {
            var data = await _tcs.Task.WaitAsync(cancellation);
            _tcs = new TaskCompletionSource<TResult>();

            return data;
        }
        catch (OperationCanceledException)
        {
            return null;
        }
    }

    public async Task<TResult?> ExecuteRemoteAsync(TArg arg, Uri? requestUri = null, CancellationToken cancellation = default)
    {
        if (_consumerHolder == null)
            _consumerHolder = await CreateConsumerAsync();

        if (requestUri == null)
        {
            await _broker.PublishAsync(arg);
        }
        else
        {
            await _broker.PublishAsync(arg, requestUri);
        }

        try
        {
            var data = await _tcs.Task.WaitAsync(cancellation);
            _tcs = new TaskCompletionSource<TResult>();

            return data;
        }
        catch (OperationCanceledException)
        {
            return null;
        }
    }

    public void Dispose()
    {
        _consumerHolder?.Dispose();
    }

    private Task<IDisposable> CreateConsumerAsync()
    {
        var lambdaConsumer = new LambdaConsumer<TResult>(OnMessageRecived);

        if (_responceUri == null)
            return _broker.SubscribeAsync(lambdaConsumer);

        return _broker.SubscribeAsync(lambdaConsumer, _responceUri);
    }

    private Task OnMessageRecived(ConsumeContext<TResult> context)
    {
        _tcs.SetResult(context.Message);
        return Task.CompletedTask;
    }
}