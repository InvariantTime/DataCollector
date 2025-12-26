using DataCollector.Server.Domain;

namespace DataCollector.Server.Infrastructure.Sessions;

public class UserSession
{
    public User User { get; }

    public Guid Id { get; }

    public DateTime LastHeartbeat { get; private set; }

    public UserSession(User user, Guid id, DateTime lastHeartbeat)
    {
        User = user;
        Id = id;
        LastHeartbeat = lastHeartbeat;
    }

    public void UpdateHeartbeat(DateTime newHeartbeat)
    {
        LastHeartbeat = newHeartbeat;
    }
}