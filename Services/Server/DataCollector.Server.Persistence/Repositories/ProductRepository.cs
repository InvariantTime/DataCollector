using DataCollector.Domain;
using DataCollector.Server.Persistence.Contexts;
using DataCollector.Shared;
using Microsoft.EntityFrameworkCore;

namespace DataCollector.Server.Persistence.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly ApplicationDbContext _context;

    public ProductRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<Product>> CreateProduct(Product product)
    {
        await _context.Products.AddAsync(product);
        await _context.SaveChangesAsync();

        return Result.Success(product);
    }

    public async Task<Result> DeleteProduct(Guid id)
    {
        var product = await _context.Products.FindAsync(id);

        if (product == null)
            return Result.Failed($"There is no product with id: {id}");

        _context.Products.Remove(product);

        await _context.SaveChangesAsync();

        return Result.Success();
    }

    public async Task<Result<Product>> GetProduct(Guid id)
    {
        var result = await _context.Products.FindAsync(id);

        if (result == null)
            return Result.Failed<Product>($"There is no product with such id: {id}");

        return Result.Success(result);
    }

    public async Task<Result<Product>> GetProductByBarcode(string barcode)
    {
        var result = await _context.Products.FirstOrDefaultAsync(x => x.BarCode == barcode);

        if (result == null)
            return Result.Failed<Product>();

        return Result.Success(result);
    }

    public async Task<Result> UpdateProduct(Product product)
    {
        var entry = _context.Entry(product);
        entry.State = EntityState.Modified;

        await _context.SaveChangesAsync();

        return Result.Success();
    }
}
