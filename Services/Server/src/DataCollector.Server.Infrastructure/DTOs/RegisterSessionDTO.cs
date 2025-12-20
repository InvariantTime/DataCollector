namespace DataCollector.Server.Infrastructure.DTOs;

public sealed record RegisterSessionDTO(string Name, string Password, Guid RequestKey);