using DataCollector.Messaging.DI;
using DataCollector.Messaging.MQTT;
using DataCollector.Shared;
using DataCollector.Terminal.App.Commands;
using DataCollector.Terminal.App.DTOs;
using DataCollector.Terminal.App.Pages;

namespace DataCollector.Terminal.App.ViewModels;

public class ConnectionPageViewModel : ViewModel
{
    private readonly MqttConnectionProvider _connectionProvider;
    private readonly MessageBrokerService _broker;

    public IAsyncCommand<ConnectionDTO> ConnectCommand =>
        field ??= AsyncCommand.Create<ConnectionDTO>(HandleCommand);

    public ConnectionPageViewModel(MqttConnectionProvider connection, MessageBrokerService broker)
    {
        _connectionProvider = connection;
        _broker = broker;
    }

    public string? Error { get; private set; }

    public bool HasError => Error != null;

    public bool IsProcessing { get; private set; }


    private async Task HandleCommand(ConnectionDTO dto)
    {
        SetProcess(true);
        var result = await ConnectAsync(dto);
        SetProcess(false);

        if (result.IsSuccess == false)
        {
            SetError(result.Error);
            return;
        }

        await NavigateAsync<RegisterPage>();
        await _broker.StartAsync();
    }

    private async Task<Result> ConnectAsync(ConnectionDTO dto)
    {
        SetProcess(true);

        var adress = dto.Address.Split(':');

        if (adress.Length != 2)
            return Result.Failed("Address is invalid");

        var host = adress[0];
        bool hasPort = int.TryParse(adress[1], out int port);

        if (hasPort == false)
            return Result.Failed("Address is invalid");

        bool result = await _connectionProvider.ApplyConnectionSettingsAsync(new MqttOptions
        {
            Host = host,
            Port = port,
            User = dto.User,
            Password = dto.Password,
        });

        if (result == false)
            return Result.Failed("Unable to connect server");

        return Result.Success();
    }

    private void SetError(string error)
    {
        Error = error;
        OnPropertyChanged(nameof(Error));
        OnPropertyChanged(nameof(HasError));
    }

    private void SetProcess(bool value)
    {
        IsProcessing = value;
        OnPropertyChanged(nameof(IsProcessing));
    }
}
