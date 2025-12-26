using DataCollector.Messaging.Core;
using DataCollector.Messaging.Core.Rpc;
using DataCollector.Shared;
using DataCollector.Shared.Messages;
using DataCollector.Terminal.App.Consumers;
using DataCollector.Terminal.App.DTOs;
using DataCollector.Terminal.App.Forms;
using DataCollector.Terminal.App.Pages;

namespace DataCollector.Terminal.App.Services;

public class RegisterService : IRegisterService
{
    private readonly ISessionProvider _session;
    private readonly IMessageBroker _broker;
    private readonly IFormService _forms;
    private readonly IDispatcher _dispatcher;

    public RegisterService(ISessionProvider session, IMessageBroker broker, IFormService forms, IDispatcher dispatcher)
    {
        _session = session;
        _forms = forms;
        _broker = broker;
        _dispatcher = dispatcher;
    }

    public async Task<Result> RegisterAsync(RegisterDTO dto, CancellationToken cancellation)
    {
        using var remote = new RemoteFunction<RegisterRequestMessage, RegisterResponceMessage>(_broker);

        var result = await remote.ExecuteRemoteAsync(new RegisterRequestMessage(dto.Name, dto.Password), cancellation);

        if (result == null)
            return Result.Failed("Unable to get responce");

        if (result.Result == false)
            return Result.Failed(result.ErrorMessage);

        var id = result.SessionId;

        var holder = new DisposeBuilder();

        var notifyConsumer = new NotifyConsumer(_forms);
        var disconnectConsumer = new DisconnectRequestConsumer(this, _forms);//TODO: session builder

        var dis1 = await _broker.SubscribeAsync(notifyConsumer, new Uri($"mqtt:devices/{id}/responce/notify"));
        var dis2 = await _broker.SubscribeAsync(disconnectConsumer, new Uri($"mqtt:devices/{id}/responce/disconnect"));

        holder.AddDisposable(dis1);
        holder.AddDisposable(dis2);

        var session = new Session(id, holder);
        session.SetCanAddProduct(result.CanPublish);
        session.SetCanUseAdminPage(result.CanUseAdminPanel);

        _session.SetSession(session);

        return Result.Success();
    }

    public async Task DisconnectAsync(bool needNotifyServer = true)
    {
       var result = _session.DeleteSession();

        if (result.IsSuccess == false)
            return;

        await _dispatcher.DispatchAsync(() =>
        {
            return Shell.Current.GoToAsync(typeof(RegisterPage).FullName);//TODO: Navigation service
        });

        await Task.Delay(100); //TODO: delay for open popup after, idea: make service for despatch navigation and popup as sequence of commands

        var id = result.Value!;

        if (needNotifyServer == true)
        {
            var uri = new Uri($"mqtt:devices/{id}/request/disconnect");
            await _broker.PublishAsync(new DisconnectRequestMessage("client disconnected"), uri);
        }
    }
}
