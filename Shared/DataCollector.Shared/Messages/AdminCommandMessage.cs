using DataCollector.Domain;

namespace DataCollector.Shared.Messages;

public record AdminCommandMessage
{
    public AdminCommandTypes Type { get; init; }

    public Guid[] Users { get; init; } = [];

    public string Message { get; init; } = string.Empty;

    public UserRoles Roles { get; init; }
}

public enum AdminCommandTypes
{
    Kick,
    Message,
    SetRole
}