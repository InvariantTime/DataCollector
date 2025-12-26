using DataCollector.Messaging.Core.Consuming;
using DataCollector.Server.Infrastructure.Sessions;
using DataCollector.Shared.Messages;
using Microsoft.Extensions.Logging;

namespace DataCollector.Server.Infrastructure.Consumers;

public class ClientHeartbeatConsumer : IMessageConsumer<ClientHeartbeatMessage>
{
    private readonly ISessionService _sessions;
    private readonly ILogger<ClientHeartbeatConsumer> _logger;

    public ClientHeartbeatConsumer(ISessionService sessions, ILogger<ClientHeartbeatConsumer> logger)
    {
        _sessions = sessions;
        _logger = logger;
    }

    public async Task ConsumeAsync(ConsumeContext<ClientHeartbeatMessage> context)
    {
        var elements = context.Route.Split('.');

        if (elements.Length < 2 || Guid.TryParse(elements[1], out var id) == false)
            return;

        bool hasSession = _sessions.Sessions.TryGetValue(id, out var session);

        if (hasSession == false)
            return;

        _logger.LogInformation("heartbeat for {0}, {1}", session!.Id, session.User.Name);
        session!.UpdateHeartbeat(DateTime.Now);
    }
}
