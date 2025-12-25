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
        OnPropertyChanged(nameof(Session));
    }

    Session ISessionProvider.CreateSession(Guid id)
    {
        Session = new Session(id);
        OnPropertyChanged(nameof(Session));

        Session.PropertyChanged += (o, e) =>
        {
            OnPropertyChanged(nameof(Session));
        };

        return Session;
    }

    private void OnPropertyChanged([CallerMemberName]string? name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
