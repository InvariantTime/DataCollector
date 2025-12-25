using DataCollector.Domain;
using DataCollector.Messaging.Core.Consuming;
using DataCollector.Server.Infrastructure.Sessions;
using DataCollector.Server.Services.Interfaces;
using DataCollector.Shared.Messages;

namespace DataCollector.Server.Infrastructure.Consumers;

public class ClientAdminCommandConsumer : IMessageConsumer<AdminCommandMessage>
{
    private readonly ISessionService _sessions;
    private readonly IUserService _users;

    public ClientAdminCommandConsumer(ISessionService sessions, IUserService users)
    {
        _sessions = sessions;
        _users = users;
    }

    public async Task ConsumeAsync(ConsumeContext<AdminCommandMessage> context)
    {
        var elements = context.Route.Split('.');

        if (elements.Length < 2 || Guid.TryParse(elements[1], out var id) == false)
            return;

        bool hasSession = _sessions.Sessions.TryGetValue(id, out var session);

        if (hasSession == false)
            return;

        var admin = session!.User;
        var adminUri = new Uri($"topic://amq.topic//devices/{id}/responce/message");

        if (admin.Role <UserRoles.Admin)
        {
            await context.PublishAsync(new NotifyClientMessage("Error", "You have not permissions"), adminUri);
            return;
        }
    }
}
