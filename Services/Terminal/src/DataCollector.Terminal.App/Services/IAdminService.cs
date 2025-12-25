using DataCollector.Domain;

namespace DataCollector.Terminal.App.Services;

public interface IAdminService
{
    Task SendMessageAsync(IEnumerable<Guid> users, string message);

    Task ChangeRoleAsync(IEnumerable<Guid> users, UserRoles role);

    Task KickAsync(IEnumerable<Guid> users);
}
