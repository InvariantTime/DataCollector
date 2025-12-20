using DataCollector.Messaging.AMQP;
using DataCollector.Messaging.Core.Consuming;
using DataCollector.Messaging.DI;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMessageBroker((builder) =>
{
    builder.AddConsumer<TestConsumer>()
        .SetEndpoint("queue://test-consumer");

    builder.UseRabbitMq((options) =>
    {
        options.User = "admin";
        options.Password = "admin123";
        options.Host = "rabbitmq";
        options.Port = 5672;
        options.VirtualHost = "/";
    });
});

var app = builder.Build();

app.Run();

class TestConsumer : IMessageConsumer<MyMessage>
{
    public Task ConsumeAsync(ConsumeContext<MyMessage> context)
    {
        return Console.Out.WriteLineAsync($"{context.Message.Value} from {context.Route}");
    }
}

record MyMessage(string Value);