using DataCollector.Server.Domain;
using Microsoft.EntityFrameworkCore;

namespace DataCollector.Server.Persistence.Contexts;

public class ApplicationDbContext : DbContext
{
    public DbSet<User> Users => Set<User>();

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        var users = modelBuilder.Entity<User>();

        users.HasKey(x => x.Id);
        users.HasAlternateKey(x => x.Name);
        users.Property(x => x.Name).HasMaxLength(30);
        users.Property(x => x.Role);
    }
}
