using DataCollector.Server.Domain;
using DataCollector.Server.Persistence.DTOs;
using DataCollector.Server.Persistence.Repositories;
using DataCollector.Server.Services.Hashing;
using DataCollector.Shared;

namespace DataCollector.Server.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _users;
    private readonly IPasswordHasher _hasher;

    public UserService(IUserRepository repository, IPasswordHasher hasher)
    {
        _users = repository;
        _hasher = hasher;
    }

    public Task<Result<User>> GetUserById(Guid id)
    {
        return _users.GetUserAsync(id);
    }

    public async Task<Result<User>> RegisterUserAsync(string name, string password)
    {
        var passwordHash = _hasher.Hash(password);

        var result = await _users.AddUserAsync(new CreateUserDTO
        {
            Name = name,
            PasswordHash = passwordHash
        });

        return result;
    }

    public async Task<Result> TryLogIn(string name, string password)
    {
        var user = await _users.GetUserByNameAsync(name);

        if (user.IsSuccess == false)
            return Result.Failed("Invalid name or login");

        var hash = user.Value!.PasswordHash;

        var result = _hasher.Verify(hash, password);

        if (result == false)
            return Result.Failed("Invalid name or login");

        return Result.Success();
    }
}
