using DataCollector.Domain;
using DataCollector.Server.Services.DTOs;
using DataCollector.Shared;

namespace DataCollector.Server.Services.Interfaces;

public interface IProductService
{
    Task<Result<Product>> AddProduct(CreateProductDTO dto);

    Task<Result<Product>> GetProductByBarCode(string barCode);

    Task<Result> RemoveProduct(Guid id);
}