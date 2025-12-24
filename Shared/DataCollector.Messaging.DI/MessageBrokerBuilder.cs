using DataCollector.Messaging.Core;
using DataCollector.Messaging.Core.Consuming;
using Microsoft.Extensions.DependencyInjection;

namespace DataCollector.Messaging.DI;

public class MessageBrokerBuilder
{
    private readonly List<Type> _consumers;
    private readonly List<MessageOptions> _endpoints;
    private Action<IServiceCollection> _connectionConfigure;

    public MessageBrokerBuilder()
    {
        _consumers = new();
        _endpoints = new();
        _connectionConfigure = delegate { };
    }

    public void UseConnection<T>() where T : class, IBrokerConnection
    {
        UseConnection((services) =>
        {
            services.AddSingleton<T>();
            services.AddSingleton<IBrokerConnection>((scope) =>
            {
                return scope.GetRequiredService<T>();
            });
        });
    }

    public void UseConnection(Action<IServiceCollection> configure)
    {
        _connectionConfigure = configure;
    }

    public void AddConsumer<T>() where T : class, IMessageConsumer
    {
        _consumers.Add(typeof(T));
    }

    public void MapConsumer<TConsumer, TMessage>(string endpoint)
        where TConsumer : class, IMessageConsumer<TMessage>
        where TMessage : class
    {
        AddConsumer<TConsumer>();
        MapEndpoint<TMessage>(endpoint);
    }

    public void MapEndpoint(Type messageType, string endpoint)
    {
        _endpoints.Add(new MessageOptions(messageType, new Uri(endpoint)));
    }

    public void MapEndpoint<T>(string endpoint) where T : class
    {
        _endpoints.Add(new MessageOptions(typeof(T), new Uri(endpoint)));
    }

    public void Build(IServiceCollection services)
    {
        _connectionConfigure.Invoke(services);
        services.AddSingleton<IConsumerCallbackFactory, DefaultCallbackFactory>();

        foreach (var consumer in _consumers)
            services.AddScoped(typeof(IMessageConsumer), consumer);

        services.AddSingleton(scope =>
        {
            var connection = scope.GetService<IBrokerConnection>();
            var factory = scope.GetService<IConsumerCallbackFactory>();

            if (connection == null)
                throw new NullReferenceException("Unable to create connection. Use AddConnection for configure");

            if (factory == null)
                throw new NullReferenceException("Unable to create callback factory");

            var consumerScope = scope.CreateScope();
            IMessageBroker broker = new MessageBroker(connection, factory, _endpoints);

            return new MessageBrokerService(broker, consumerScope);
        });

        services.AddSingleton<IMessageBroker>(scope =>
        {
            return scope.GetRequiredService<MessageBrokerService>();
        });
    }
}