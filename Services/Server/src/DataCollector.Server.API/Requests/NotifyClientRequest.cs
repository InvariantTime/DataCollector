namespace DataCollector.Server.API.Requests;

public record NotifyClientRequest(Guid Id, string Title, string Content);