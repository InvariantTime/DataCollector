using DataCollector.Messaging.AMQP;
using DataCollector.Messaging.Core.Consuming;
using DataCollector.Messaging.DI;
using DataCollector.Server.API;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMessageBroker((builder) =>
{

    builder.UseRabbitMq((options) =>
    {
        options.User = "admin";
        options.Password = "admin123";
        options.Host = "rabbitmq";
        options.Port = 5672;
        options.VirtualHost = "/";
    });
});

builder.Services.AddHostedService<BrokerStartupService>();

var app = builder.Build();

app.Run();