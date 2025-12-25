using DataCollector.Domain;
using DataCollector.Messaging.Core;

namespace DataCollector.Terminal.App.Services;

public class AdminService : IAdminService
{
    private readonly IMessageBroker _broker;
    private readonly ISessionProvider _session;

    public AdminService(ISessionProvider session, IMessageBroker broker)
    {
        _session = session;
        _broker = broker;
    }

    public Task ChangeRoleAsync(IEnumerable<Guid> users, UserRoles role)
    {
        throw new NotImplementedException();
    }

    public Task KickAsync(IEnumerable<Guid> users)
    {
        throw new NotImplementedException();
    }

    public Task SendMessageAsync(IEnumerable<Guid> users, string message)
    {
        throw new NotImplementedException();
    }
}
