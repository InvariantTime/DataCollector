using DataCollector.Domain;
using DataCollector.Messaging.Core;
using DataCollector.Messaging.Core.Rpc;
using DataCollector.Shared.Messages;
using DataCollector.Terminal.App.DTOs;

namespace DataCollector.Terminal.App.Services;

public class AdminService : IAdminService
{
    private static readonly TimeSpan _remoteTimeout = TimeSpan.FromSeconds(5);

    private readonly IMessageBroker _broker;
    private readonly ISessionProvider _session;

    public AdminService(ISessionProvider session, IMessageBroker broker)
    {
        _session = session;
        _broker = broker;
    }

    public Task<NotifyMessageDTO> ChangeRoleAsync(IEnumerable<Guid> users, UserRoles role)
    {
        var command = AdminCommandMessage.CreateChangeRoleCommand(role, users);
        return ExecuteCommonCommandAsync(command);
    }

    public Task<NotifyMessageDTO> KickAsync(IEnumerable<Guid> users)
    {
        var command = AdminCommandMessage.CreateKickCommand(users);
        return ExecuteCommonCommandAsync(command);
    }

    public Task<NotifyMessageDTO> SendMessageAsync(IEnumerable<Guid> users, string message)
    {
        var command = AdminCommandMessage.CreateMessageCommand(message, users);
        return ExecuteCommonCommandAsync(command);
    }

    private async Task<NotifyMessageDTO> ExecuteCommonCommandAsync(AdminCommandMessage command)
    {
        var session = _session.Session;

        if (session == null)
            return new NotifyMessageDTO("Error", "Unable to get session", NotifyTypes.Error);

        var cancellation = new CancellationTokenSource(_remoteTimeout);

        var requestUri = session.CreateRequestUri("admincommand");
        var responceUri = session.CreateResponceUri("message");

        using var remote = new RemoteFunction<AdminCommandMessage, NotifyClientMessage>(_broker, responceUri);
        var result = await remote.ExecuteRemoteAsync(command, requestUri, cancellation.Token);

        if (result == null)
            return new NotifyMessageDTO("Error", "Unable to get responce", NotifyTypes.Error);

        return new NotifyMessageDTO(result.Title, result.Content, NotifyTypes.Message);
    }
}
