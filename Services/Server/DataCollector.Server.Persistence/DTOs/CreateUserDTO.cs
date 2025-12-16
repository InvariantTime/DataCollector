using DataCollector.Server.Domain;

namespace DataCollector.Server.Persistence.DTOs;

public readonly record struct CreateUserDTO(string Name, string PasswordHash);