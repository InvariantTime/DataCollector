using DataCollector.Messaging.Core.Consuming;
using DataCollector.Server.Infrastructure.Sessions;
using DataCollector.Server.Services.Interfaces;
using DataCollector.Shared.Messages;

namespace DataCollector.Server.Infrastructure.Consumers;

public class RegisterRequestConsumer : IMessageConsumer<RegisterRequestMessage>
{
    private readonly ISessionService _sessions;
    private readonly IUserService _users;

    public RegisterRequestConsumer(ISessionService sessions, IUserService users)
    {
        _sessions = sessions;
        _users = users;
    }

    public async Task ConsumeAsync(ConsumeContext<RegisterRequestMessage> context)
    {
        var message = context.Message;

        var result = await _users.TryLogInAsync(message.Name, message.Password);

        if (result.IsSuccess == false)
        {
            await context.PublishAsync(RegisterResponceMessage.Error(result.Error));
            return;
        }

        var user = result.Value!;
        var id = await _sessions.ConnectSessionAsync(user);

        if (id.IsSuccess == false)
        {
            await context.PublishAsync(RegisterResponceMessage.Error(id.Error));
            return;
        }

        await context.PublishAsync(RegisterResponceMessage.Success(id.Value));
    }
}
