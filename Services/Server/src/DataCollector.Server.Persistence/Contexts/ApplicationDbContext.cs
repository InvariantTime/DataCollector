using DataCollector.Domain;
using DataCollector.Server.Domain;
using Microsoft.EntityFrameworkCore;

namespace DataCollector.Server.Persistence.Contexts;

public class ApplicationDbContext : DbContext
{
    public DbSet<User> Users => Set<User>();

    public DbSet<Product> Products => Set<Product>();

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        var users = modelBuilder.Entity<User>();
        var products = modelBuilder.Entity<Product>();

        users.HasKey(x => x.Id);
        users.HasAlternateKey(x => x.Name);
        users.Property(x => x.Name).HasMaxLength(30);
        users.Property(x => x.Role);

        products.HasKey(x => x.Id);
        products.HasAlternateKey(x => x.BarCode);
        products.Property(x => x.Description).HasMaxLength(120);
        products.Property(x => x.BarCode).HasMaxLength(120);
        products.Property(x => x.Name).HasMaxLength(50);
    }
}
