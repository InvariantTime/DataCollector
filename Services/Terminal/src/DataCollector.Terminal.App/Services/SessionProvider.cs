using DataCollector.Messaging.Core;
using DataCollector.Terminal.App.Consumers;
using DataCollector.Terminal.App.Forms;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DataCollector.Terminal.App.Services;

public class SessionProvider : ISessionProvider, INotifyPropertyChanged
{
    private readonly IMessageBroker _broker;
    private readonly IFormService _forms;

    public Session? Session { get; private set; }

    public event PropertyChangedEventHandler? PropertyChanged;

    public SessionProvider(IMessageBroker broker, IFormService forms)
    {
        _broker = broker;
        _forms = forms;
    }

    public void DeleteSession()
    {
        Session = null;
        OnPropertyChanged(nameof(Session));
    }

    public Session CreateSession(Guid id)
    {
        Session?.Dispose();

        var notifyConsumer = new NotifyConsumer(_forms);
        var holder = _broker.SubscribeAsync(notifyConsumer, new Uri($"services/{id}/responce/notify"))
            .GetAwaiter()
            .GetResult();

        Session = new Session(id, holder);
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
