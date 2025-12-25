using DataCollector.Domain;

namespace DataCollector.Shared.Messages;

public record AdminCommandMessage
{
    public AdminCommandTypes Type { get; init; }

    public Guid[] Users { get; init; } = [];

    public string Message { get; init; } = string.Empty;

    public UserRoles Roles { get; init; }

    public static AdminCommandMessage CreateKickCommand(IEnumerable<Guid> users)
    {
        return new AdminCommandMessage
        {
            Users = users.ToArray(),
            Type = AdminCommandTypes.Kick
        };
    }

    public static AdminCommandMessage CreateMessageCommand(string message, IEnumerable<Guid> users)
    {
        return new AdminCommandMessage
        {
            Users = users.ToArray(),
            Type = AdminCommandTypes.Message,
            Message = message
        };
    }

    public static AdminCommandMessage CreateChangeRoleCommand(UserRoles role, IEnumerable<Guid> users)
    {
        return new AdminCommandMessage
        {
            Users = users.ToArray(),
            Type = AdminCommandTypes.SetRole,
            Roles = role
        };
    }
}

public enum AdminCommandTypes
{
    Kick,
    Message,
    SetRole
}