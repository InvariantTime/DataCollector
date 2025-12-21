
namespace DataCollector.Messaging.Core.Consuming;

public class LambdaConsumer<T> : IMessageConsumer<T> where T : class
{
    private readonly Func<ConsumeContext<T>, Task> _lambda;

    public LambdaConsumer(Func<ConsumeContext<T>, Task> lambda)
    {
        _lambda = lambda;
    }

    public Task ConsumeAsync(ConsumeContext<T> context)
    {
        return _lambda.Invoke(context);
    }
}
