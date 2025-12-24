namespace DataCollector.Shared.Messages;

public class RegisterResponceMessage
{
    public bool Result { get; init; }

    public string? ErrorMessage { get; init; }

    public Guid SessionId { get; init; }

    public bool CanPublish { get; init; }

    public bool CanUseAdminPanel { get; init; }

    public static RegisterResponceMessage Error(string message)
    {
        return new RegisterResponceMessage
        {
            Result = false,
            ErrorMessage = message
        };
    }

    public static RegisterResponceMessage Success(Guid id, bool canPublish = false, bool canUseAdminPanel = false)
    {
        return new RegisterResponceMessage
        {
            Result = true,
            SessionId = id,
            CanPublish = canPublish,
            CanUseAdminPanel = canUseAdminPanel
        };
    }
}