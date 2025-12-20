
namespace DataCollector.Server.Domain;

public class User
{
    public Guid Id { get; set; } = Guid.Empty;

    public string Name { get; set; } = string.Empty;

    public string PasswordHash { get; set; } = string.Empty;

    public UserRoles Role { get; set; } = UserRoles.Scanner;
}
