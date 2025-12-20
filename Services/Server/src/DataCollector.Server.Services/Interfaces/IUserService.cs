using DataCollector.Server.Domain;
using DataCollector.Shared;

namespace DataCollector.Server.Services.Interfaces;

public interface IUserService
{
    Task<Result<User>> RegisterUserAsync(string name, string password);

    Task<Result> TryLogIn(string name, string password);

    Task<Result<User>> GetUserById(Guid id);
}
