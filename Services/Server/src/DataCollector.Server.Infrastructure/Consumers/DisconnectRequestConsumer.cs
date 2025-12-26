using DataCollector.Messaging.Core.Consuming;
using DataCollector.Server.Infrastructure.Sessions;
using DataCollector.Shared.Messages;
using Microsoft.Extensions.Logging;

namespace DataCollector.Server.Infrastructure.Consumers;

public class DisconnectRequestConsumer : IMessageConsumer<DisconnectRequestMessage>
{
    private readonly ISessionService _sessions;
    private readonly ILogger<DisconnectRequestConsumer> _logger;

    public DisconnectRequestConsumer(ISessionService sessions, ILogger<DisconnectRequestConsumer> logger)
    {
        _sessions = sessions;
        _logger = logger;
    }

    public async Task ConsumeAsync(ConsumeContext<DisconnectRequestMessage> context)
    {
        var elements = context.Route.Split('.');

        if (elements.Length < 2 || Guid.TryParse(elements[1], out var id) == false)
            return;

        bool hasSession = _sessions.Sessions.TryGetValue(id, out var session);

        if (hasSession == false)
            return;

        _logger.LogInformation("Disconnect {0}, Name: {1} with reason: {2}", session!.Id, session.User.Name, context.Message.Reason);
        await _sessions.DisconnectSessionAsync(session!.Id, false);
    }
}
