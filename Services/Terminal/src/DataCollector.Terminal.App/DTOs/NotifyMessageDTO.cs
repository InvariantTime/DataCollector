namespace DataCollector.Terminal.App.DTOs;

public record NotifyMessageDTO(string Title, string Content, NotifyTypes Type);

public enum NotifyTypes
{
    Error,
    Message
}