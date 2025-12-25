using DataCollector.Domain;
using DataCollector.Messaging.Core.Consuming;
using DataCollector.Server.Infrastructure.Sessions;
using DataCollector.Server.Services.Interfaces;
using DataCollector.Shared.Messages;
using Microsoft.Extensions.Logging;

namespace DataCollector.Server.Infrastructure.Consumers;

public class RegisterRequestConsumer : IMessageConsumer<RegisterRequestMessage>
{
    private readonly ISessionService _sessions;
    private readonly IUserService _users;
    private readonly ILogger<RegisterRequestConsumer> _logger;

    public RegisterRequestConsumer(ISessionService sessions, IUserService users, ILogger<RegisterRequestConsumer> logger)
    {
        _sessions = sessions;
        _users = users;
        _logger = logger;
    }

    public async Task ConsumeAsync(ConsumeContext<RegisterRequestMessage> context)
    {
        _logger.LogInformation($"Handle {nameof(RegisterRequestMessage)}");

        var message = context.Message;
        var result = await _users.TryLogInAsync(message.Name, message.Password);

        if (result.IsSuccess == false)
        {
            _logger.LogInformation("Client failed to login");
            await context.PublishAsync(RegisterResponceMessage.Error(result.Error));
            return;
        }

        _logger.LogInformation("Client success to login");

        var user = result.Value!;
        var id = await _sessions.ConnectSessionAsync(user);

        if (id.IsSuccess == false)
        {
            _logger.LogInformation("Client failed to create session");
            await context.PublishAsync(RegisterResponceMessage.Error(id.Error));
            return;
        }

        _logger.LogInformation("Client success to create session");

        bool canPublish = user.Role >= UserRoles.Publisher;
        bool admin = user.Role == UserRoles.Admin;

        await context.PublishAsync(RegisterResponceMessage.Success(id.Value, canPublish, admin));
    }
}
