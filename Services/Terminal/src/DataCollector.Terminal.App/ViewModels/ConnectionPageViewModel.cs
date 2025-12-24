using DataCollector.Messaging.MQTT;
using DataCollector.Terminal.App.Commands;
using DataCollector.Terminal.App.DTOs;

namespace DataCollector.Terminal.App.ViewModels;

public class ConnectionPageViewModel : ViewModel
{
    private MqttConnectionProvider _connectionProvider;

    public IAsyncCommand<ConnectionDTO> ConnectCommand =>
        field ??= AsyncCommand.Create<ConnectionDTO>(ConnectAsync);

    public ConnectionPageViewModel(MqttConnectionProvider connection)
    {
        _connectionProvider = connection;
    }

    public string? Error { get; private set; }

    public bool HasError => Error != null;

    private Task ConnectAsync(ConnectionDTO dto)
    {
        var adress = dto.Address.Split(':');

        if (adress.Length != 2)
            return WithError("Address is invalid");

        var host = adress[0];
        bool hasPort = int.TryParse(adress[1], out int port);

        if (hasPort == false)
            return WithError("Address is invalid");

        bool result = _connectionProvider.ApplyConnectionSettings(new MqttOptions
        {
            Host = host,
            Port = port,
            User = dto.User,
            Password = dto.Password,
        });

        if (result == false)
            return WithError("Unable to connect server");

        return Task.CompletedTask;
    }

    public Task WithError(string error)
    {
        Error = error;
        OnPropertyChanged(nameof(Error));
        OnPropertyChanged(nameof(HasError));
        return Task.CompletedTask;
    }
}
