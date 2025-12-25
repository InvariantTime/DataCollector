using DataCollector.Domain;
using DataCollector.Terminal.App.DTOs;

namespace DataCollector.Terminal.App.Services;

public interface IAdminService
{
    Task<NotifyMessageDTO> SendMessageAsync(IEnumerable<Guid> users, string message);

    Task<NotifyMessageDTO> ChangeRoleAsync(IEnumerable<Guid> users, UserRoles role);

    Task<NotifyMessageDTO> KickAsync(IEnumerable<Guid> users);
}
