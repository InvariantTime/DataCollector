using DataCollector.Server.Domain;
using DataCollector.Shared;

namespace DataCollector.Server.Infrastructure.Sessions;

public interface ISessionService
{
    IReadOnlyDictionary<Guid, UserSession> Sessions { get; }

    Task<Result<Guid>> ConnectSessionAsync(User user);

    Task<Result> DisconnectSessionAsync(Guid sessionId, bool needNotifyClient = true, string? reason = null);
}