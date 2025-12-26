namespace DataCollector.Server.API.Requests;

public record KickClientRequest(Guid Id, string? Reason = null);