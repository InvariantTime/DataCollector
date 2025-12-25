namespace DataCollector.Terminal.App.Services;

public interface ISessionProvider
{
    Session? Session { get; }

    Session CreateSession(Guid id);

    void DeleteSession();
}