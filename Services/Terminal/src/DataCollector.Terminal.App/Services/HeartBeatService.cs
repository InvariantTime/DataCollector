
using DataCollector.Messaging.Core;
using DataCollector.Shared.Messages;

namespace DataCollector.Terminal.App.Services;

public class HeartBeatService : IBackgroundService
{
    private static readonly TimeSpan _heartbeatPeriod = TimeSpan.FromSeconds(15);
    private static readonly ClientHeartbeatMessage _heartbeat = new ClientHeartbeatMessage(0);

    private readonly IMessageBroker _broker;
    private readonly ISessionProvider _sessions;
    private readonly IRegisterService _register;

    public HeartBeatService(IMessageBroker broker, ISessionProvider sessions, IRegisterService register)
    {
        _broker = broker;
        _sessions = sessions;
        _register = register;
    }

    public async Task ExecuteAsync(CancellationToken cancellation)
    {
        using var timer = new PeriodicTimer(_heartbeatPeriod);

        while (await timer.WaitForNextTickAsync(cancellation) == true)
        {
            var session = _sessions.Session;

            if (session == null)
                continue;

            var uri = session.CreateRequestUri("heartbeat");
            await _broker.PublishAsync(_heartbeat, uri);
        }
    }

    public Task CloseAsync()
    {
        return _register.DisconnectAsync(true);
    }
}
