using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DataCollector.Server.Infrastructure.Sessions;

public class SessionHealthCheckService : BackgroundService
{
    private readonly ISessionService _sessions;
    private readonly UserSessionOptions _options;
    private readonly ILogger<SessionHealthCheckService> _logger;

    public SessionHealthCheckService(IOptions<UserSessionOptions> options, 
        ISessionService sessions, ILogger<SessionHealthCheckService> logger)
    {
        _sessions = sessions;
        _options = options.Value;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken cancellation)
    {
        using var timer = new PeriodicTimer(TimeSpan.FromSeconds(_options.HealthCheckPeriod));

        var maxUnhealedLive = TimeSpan.FromSeconds(_options.MaxUnhealedPeriod);

        while (await timer.WaitForNextTickAsync(cancellation) == true)
        {
            var now = DateTime.Now;
            var sessions = _sessions.Sessions.Where(x => now - x.Value.LastHeartbeat >= maxUnhealedLive);

            foreach (var session in sessions)
            {
                _logger.LogInformation("Disconnect unhealth client {0}, Name: {1}", session.Key, session.Value.User.Name);
                await _sessions.DisconnectSessionAsync(session.Key);
            }
        }
    }
}