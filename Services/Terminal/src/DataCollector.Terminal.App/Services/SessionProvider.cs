


using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DataCollector.Terminal.App.Services;

public class SessionProvider : ISessionProvider, INotifyPropertyChanged
{
    public Session? Session { get; private set; }

    public event PropertyChangedEventHandler? PropertyChanged;

    public void DeleteSession()
    {
        Session = null;
    }

    Session ISessionProvider.CreateSession(Guid id)
    {
        var session = new Session(id);
        OnPropertyChanged(nameof(session));

        return session;
    }

    private void OnPropertyChanged([CallerMemberName]string? name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
