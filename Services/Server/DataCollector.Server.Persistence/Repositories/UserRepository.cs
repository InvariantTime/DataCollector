using DataCollector.Server.Domain;
using DataCollector.Server.Persistence.Contexts;
using DataCollector.Shared;
using Microsoft.EntityFrameworkCore;

namespace DataCollector.Server.Persistence.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;

    public UserRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<User>> AddUserAsync(User user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        return Result.Success(user);
    }

    public async Task<Result<User>> GetUserAsync(Guid id)
    {
        var user = await _context.Users.FindAsync(id);

        if (user == null)
            return Result.Failed<User>($"There is no such user with id: {id}");

        return Result.Success(user);
    }

    public async Task<Result<User>> GetUserByNameAsync(string name)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Name == name);

        if (user == null)
            return Result.Failed<User>($"There is no such user with name '{name}'");

        return Result.Success(user);
    }

    public async Task<Result> UpdateUserAsync(User user)
    {
        _context.Entry(user)
            .State = EntityState.Modified;

       await _context.SaveChangesAsync();

        return Result.Success();
    }
}