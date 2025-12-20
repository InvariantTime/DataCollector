using Microsoft.Extensions.DependencyInjection;

namespace DataCollector.Messaging.DI;

public static class BrokerServicesExtensions
{
    public static void AddMessageBroker(this IServiceCollection services, Action<MessageBrokerBuilder> buildAction)
    {
        var builder = new MessageBrokerBuilder();
        buildAction.Invoke(builder);

        builder.Build(services);
    }
}
