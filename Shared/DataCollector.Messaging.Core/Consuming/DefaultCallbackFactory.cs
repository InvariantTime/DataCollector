using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.Json;

namespace DataCollector.Messaging.Core.Consuming;

public class DefaultCallbackFactory : IConsumerCallbackFactory
{
    private static readonly MethodInfo _consumerMethod = typeof(IMessageConsumer<>)
        .GetMethod(nameof(IMessageConsumer<>.ConsumeAsync))!;

    private static readonly MethodInfo _createCallbackMethod = typeof(DefaultCallbackFactory)
        .GetMethod(nameof(CreateCallbackInternal), BindingFlags.NonPublic | BindingFlags.Static)!;


    private readonly ConcurrentDictionary<Type, Func<IMessageConsumer, ConsumerCallback>> _consumerCallbackBuilders = new();

    public ConsumerCallback CreateCallback(IMessageConsumer consumer)
    {
        var builder = _consumerCallbackBuilders.GetOrAdd(consumer.GetType(), CreateBuilder);
        return builder.Invoke(consumer);
    }

    public ConsumerCallback CreateCallback<T>(IMessageConsumer<T> consumer) where T : class
    {
        return CreateCallbackInternal(consumer);
    }

    private Func<IMessageConsumer, ConsumerCallback> CreateBuilder(Type consumerType)
    {
        var consumerImplementation = consumerType.GetInterfaces()
            .Single(x => x.IsGenericType == true && x.GetGenericTypeDefinition() == typeof(IMessageConsumer<>));

        var generic = consumerImplementation.GetGenericArguments().Single();
        var method = _createCallbackMethod.MakeGenericMethod(generic);

        var consumerParam = Expression.Parameter(typeof(IMessageConsumer), "consumer");
        var call = Expression.Call(method, Expression.Convert(consumerParam, consumerImplementation));
        var lambda = Expression.Lambda<Func<IMessageConsumer, ConsumerCallback>>(call, consumerParam);

        return lambda.Compile();
    }

    private static ConsumerCallback CreateCallbackInternal<T>(IMessageConsumer<T> consumer)
        where T : class
    {
        return (data, broker) =>
        {
            var payload = JsonSerializer.Deserialize<T>(data.Payload);

            if (payload == null)
                throw new InvalidOperationException("Unable to deserialize data");

            var context = new ConsumeContext<T>(broker, payload, data.Route);

            return consumer.ConsumeAsync(context);
        };
    }
}
