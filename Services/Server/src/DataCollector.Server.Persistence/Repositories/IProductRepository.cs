using DataCollector.Domain;
using DataCollector.Shared;

namespace DataCollector.Server.Persistence.Repositories;

public interface IProductRepository
{
    Task<Result<Product>> CreateProduct(Product product);

    Task<Result<Product>> GetProductByBarcode(string barcode);

    Task<Result<Product>> GetProduct(Guid id);

    Task<Result> DeleteProduct(Guid id);

    Task<Result> UpdateProduct(Product product);
}
