using DataCollector.Messaging.Core;
using DataCollector.Shared;
using DataCollector.Terminal.App.Consumers;
using DataCollector.Terminal.App.Forms;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DataCollector.Terminal.App.Services;

public class SessionProvider : ISessionProvider, INotifyPropertyChanged
{
    public Session? Session { get; private set; }

    public event PropertyChangedEventHandler? PropertyChanged;

    public Result<Guid> DeleteSession()
    {
        if (Session == null)
            return Result.Failed<Guid>();

        var id = Session.Id;

        Session.Dispose();
        Session = null;

        OnPropertyChanged(nameof(Session));

        return Result.Success(id);
    }

    public void SetSession(Session session)
    {
        Session?.Dispose();

        Session = session;
        OnPropertyChanged(nameof(Session));

        Session.PropertyChanged += (o, e) =>
        {
            OnPropertyChanged(nameof(Session));
        };
    }

    private void OnPropertyChanged([CallerMemberName]string? name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
