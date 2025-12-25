using DataCollector.Domain;

namespace DataCollector.Server.API.Requests;

public record RegisterUserRequest(string Name, string Password, UserRoles Role);