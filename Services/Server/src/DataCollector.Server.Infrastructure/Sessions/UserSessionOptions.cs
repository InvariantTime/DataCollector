namespace DataCollector.Server.Infrastructure.Sessions;

public class UserSessionOptions
{
    public int HealthCheckPeriod { get; init; } = 15;

    public int MaxUnhealedPeriod { get; init; } = 40;
}
