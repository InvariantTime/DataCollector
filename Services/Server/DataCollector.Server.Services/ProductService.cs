using DataCollector.Domain;
using DataCollector.Server.Persistence.Repositories;
using DataCollector.Server.Services.DTOs;
using DataCollector.Server.Services.Interfaces;
using DataCollector.Shared;

namespace DataCollector.Server.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _products;

    public ProductService(IProductRepository products)
    {
        _products = products;
    }

    public async Task<Result<Product>> AddProduct(CreateProductDTO dto)
    {
        var product = new Product
        {
            BarCode = dto.BarCode,
            Name = dto.Name,
            Description = dto.Description,
        };

        var other = await _products.GetProductByBarcode(dto.BarCode);

        if (other.IsSuccess == true)
            return Result.Failed<Product>($"Product for barcode {dto.BarCode} is already exists");

        var result = await _products.CreateProduct(product);

        if (result.IsSuccess == false)
            return Result.Failed<Product>(result.Error);

        return Result.Success(product);
    }

    public Task<Result<Product>> GetProductByBarCode(string barCode)
    {
        return _products.GetProductByBarcode(barCode);
    }

    public Task<Result> RemoveProduct(Guid id)
    {
        return _products.DeleteProduct(id);
    }
}