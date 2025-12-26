using DataCollector.Shared;

namespace DataCollector.Terminal.App.Services;

public interface ISessionProvider
{
    Session? Session { get; }

    void SetSession(Session session);

    Result<Guid> DeleteSession();
}