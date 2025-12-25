using DataCollector.Messaging.Core;
using DataCollector.Messaging.Core.Rpc;
using DataCollector.Shared;
using DataCollector.Shared.Messages;
using DataCollector.Terminal.App.DTOs;

namespace DataCollector.Terminal.App.Services;

public class RegisterService : IRegisterService
{
    private readonly ISessionProvider _session;
    private readonly IMessageBroker _broker;

    public RegisterService(ISessionProvider session, IMessageBroker broker)
    {
        _session = session;
        _broker = broker;
    }

    public async Task<Result> RegisterAsync(RegisterDTO dto, CancellationToken cancellation)
    {
        using var remote = new RemoteFunction<RegisterRequestMessage, RegisterResponceMessage>(_broker);

        var result = await remote.ExecuteRemoteAsync(new RegisterRequestMessage(dto.Name, dto.Password), cancellation);


        if (result.Result == false)
            return Result.Failed(result.ErrorMessage);

        var session = _session.CreateSession(result.SessionId);
        session.SetCanAddProduct(result.CanPublish);
        session.SetCanUseAdminPage(result.CanUseAdminPanel);

        return Result.Success();
    }

    public async Task DisconnectAsync()
    {


        _session.DeleteSession();
    }
}
