using DataCollector.Server.Domain;
using DataCollector.Shared;

namespace DataCollector.Server.Infrastructure.Sessions;

public interface ISessionService
{
    IReadOnlyCollection<UserSession> Sessions { get; }

    Task<Result> ConnectSessionAsync(User user);

    Task<Result> DisconnectSessionAsync(Guid sessionId);

    Task<Result> SendMessageAsync(Guid sessionId);
}