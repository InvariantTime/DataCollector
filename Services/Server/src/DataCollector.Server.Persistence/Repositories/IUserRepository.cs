using DataCollector.Server.Domain;
using DataCollector.Shared;

namespace DataCollector.Server.Persistence.Repositories;

public interface IUserRepository
{
    Task<Result<User>> AddUserAsync(User user);

    Task<Result<User>> GetUserByNameAsync(string name);

    Task<Result> UpdateUserAsync(User user);
    
    Task<Result<User>> GetUserAsync(Guid id);
}
