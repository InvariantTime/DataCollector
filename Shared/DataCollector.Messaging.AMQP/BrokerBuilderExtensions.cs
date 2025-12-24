using DataCollector.Messaging.Core;
using DataCollector.Messaging.DI;
using Microsoft.Extensions.DependencyInjection;

namespace DataCollector.Messaging.AMQP;

public static class BrokerBuilderExtensions
{
    public static void UseRabbitMq(this MessageBrokerBuilder builder, Action<RabbitMQOptions> optionsAction)
    {
        builder.UseConnection((services) =>
        {
            var options = new RabbitMQOptions();
            optionsAction.Invoke(options);

            services.AddSingleton<IBrokerConnection, RabbitMQConnection>((_) =>
            {
                return new RabbitMQConnection(options);
            });
        });
    }

    public static void UseRabbitMq(this MessageBrokerBuilder builder, Func<RabbitMQOptions> optionsFunc)
    {
        builder.UseConnection((services) =>
        {
            var options = optionsFunc.Invoke();

            services.AddSingleton<IBrokerConnection, RabbitMQConnection>((_) =>
            {
                return new RabbitMQConnection(options);
            });
        });
    }
}
