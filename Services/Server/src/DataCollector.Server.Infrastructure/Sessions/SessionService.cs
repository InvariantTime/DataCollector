using DataCollector.Server.Domain;
using DataCollector.Shared;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;

namespace DataCollector.Server.Infrastructure.Sessions;

public class SessionService : ISessionService
{
    private readonly ConcurrentDictionary<Guid, UserSession> _sessions;

    public IReadOnlyDictionary<Guid, UserSession> Sessions =>
        field ??= new ReadOnlyDictionary<Guid, UserSession>(_sessions);

    public SessionService()
    {
        _sessions = new ConcurrentDictionary<Guid, UserSession>();
    }

    public Task<Result<Guid>> ConnectSessionAsync(User user)
    {
        var guid = Guid.NewGuid();

        var session = new UserSession(user, guid);
        _sessions.TryAdd(guid, session);

        return Task.FromResult(Result.Success(guid));
    }

    public Task<Result> DisconnectSessionAsync(Guid sessionId)
    {
        bool result = _sessions.TryRemove(sessionId, out var _);

        if (result == false)
            return Task.FromResult(Result.Failed());

        return Task.FromResult(Result.Success());
    }
}
