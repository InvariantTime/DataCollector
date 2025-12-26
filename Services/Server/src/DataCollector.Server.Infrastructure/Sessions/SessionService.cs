using DataCollector.Messaging.Core;
using DataCollector.Server.Domain;
using DataCollector.Shared;
using DataCollector.Shared.Messages;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;

namespace DataCollector.Server.Infrastructure.Sessions;

public class SessionService : ISessionService
{
    private readonly ConcurrentDictionary<Guid, UserSession> _sessions;
    private readonly IMessageBroker _broker;

    public IReadOnlyDictionary<Guid, UserSession> Sessions =>
        field ??= new ReadOnlyDictionary<Guid, UserSession>(_sessions);

    public SessionService(IMessageBroker broker)
    {
        _sessions = new ConcurrentDictionary<Guid, UserSession>();
        _broker = broker;
    }

    public Task<Result<Guid>> ConnectSessionAsync(User user)
    {
        var guid = Guid.NewGuid();

        var session = new UserSession(user, guid, DateTime.Now);
        _sessions.TryAdd(guid, session);

        return Task.FromResult(Result.Success(guid));
    }

    public Task<Result> DisconnectSessionAsync(Guid sessionId, bool needNotifyClient = true, string? reason = null)
    {
        bool result = _sessions.TryRemove(sessionId, out var _);

        if (result == false)
            return Task.FromResult(Result.Failed());

        if (needNotifyClient == true)
        {
            var uri = new Uri($"topic://amq.topic/devices/{sessionId}/responce/disconnect?durable=true");
            _broker.PublishAsync(new DisconnectRequestMessage(reason ?? "disconnect by server"), uri);
        }

        return Task.FromResult(Result.Success());
    }
}
