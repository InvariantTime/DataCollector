using DataCollector.Server.Domain;

namespace DataCollector.Server.Infrastructure.Sessions;

public class UserSession
{
    public User User { get; }

    public Guid Id { get; }

    public UserSession(User user, Guid id)
    {
        User = user;
        Id = id;
    }
}
