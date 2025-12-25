using DataCollector.Messaging.AMQP;
using DataCollector.Messaging.DI;
using DataCollector.Server.API;
using DataCollector.Server.API.Requests;
using DataCollector.Server.Infrastructure.Consumers;
using DataCollector.Server.Infrastructure.Sessions;
using DataCollector.Server.Persistence.Contexts;
using DataCollector.Server.Persistence.Repositories;
using DataCollector.Server.Services;
using DataCollector.Server.Services.Hashing;
using DataCollector.Server.Services.Interfaces;
using DataCollector.Shared.Messages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("postgres");

builder.Services.AddDbContext<ApplicationDbContext>(op =>
{
    op.UseNpgsql(connectionString);
});


builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddSingleton<ISessionService, SessionService>();
builder.Services.AddSingleton<IPasswordHasher, DefaultHasher>();

builder.Services.AddMessageBroker((broker) =>
{
    //Consumers
    broker.MapConsumer<RegisterRequestConsumer, RegisterRequestMessage>("topic://amq.topic/devices/register/request?durable=true");
    broker.MapConsumer<ClientScanMessageConsumer, ScanMessage>("topic://amq.topic/devices/*/request/scan?durable=true");
    broker.MapConsumer<ClientAddProductMessageConsumer, AddProductMessage>("topic://amq.topic/devices/*/request/add?durable=true");
    broker.MapConsumer<ClientAdminCommandConsumer, AdminCommandMessage>("topic://amq.topic/devices/*/request/admin?durable=true");

    //Messages
    broker.MapEndpoint<RegisterResponceMessage>("topic://amq.topic/devices/register/responce?durable=true");

    broker.UseRabbitMq(() =>
    {
        var options = builder.Configuration.GetSection("Rabbitmq").Get<RabbitMQOptions>()
            ?? throw new NullReferenceException("Unable to get rabbitmq options");

        return options;
    });
});

builder.Services.AddHostedService<BrokerStartupService>();

var app = builder.Build();

app.MapPost("api/users/register", async ([FromBody]RegisterUserRequest request, IUserService users) =>
{
    var user = await users.RegisterUserAsync(request.Name, request.Password);

    if (user.IsSuccess == false)
        return user.Error;

    user.Value!.Role = request.Role;
    await users.UpdateUser(user.Value);

    return "success!";
});

app.Run();