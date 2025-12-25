using DataCollector.Domain;

namespace DataCollector.Terminal.App.DTOs;

public record UserDTO(Guid Id, string Name, UserRoles Role);