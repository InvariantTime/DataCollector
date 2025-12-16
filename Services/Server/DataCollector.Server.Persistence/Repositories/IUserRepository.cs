using DataCollector.Server.Domain;
using DataCollector.Server.Persistence.DTOs;
using DataCollector.Shared;

namespace DataCollector.Server.Persistence.Repositories;

public interface IUserRepository
{
    Task<Result<User>> AddUserAsync(CreateUserDTO creation);

    Task<Result<User>> GetUserByNameAsync(string name);

    Task<Result> UpdateUserAsync(User user);
    
    Task<Result<User>> GetUserAsync(Guid id);
}
