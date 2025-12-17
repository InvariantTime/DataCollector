using DataCollector.Server.Domain;
using DataCollector.Server.Persistence.Repositories;
using DataCollector.Server.Services.Hashing;
using DataCollector.Server.Services.Interfaces;
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

    public Task<Result<User>> RegisterUserAsync(string name, string password)
    {
        var passwordHash = _hasher.Hash(password);

        var user = new User
        {
            Name = name,
            PasswordHash = passwordHash,
            Role = UserRoles.Scanner
        };

        return _users.AddUserAsync(user);
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
