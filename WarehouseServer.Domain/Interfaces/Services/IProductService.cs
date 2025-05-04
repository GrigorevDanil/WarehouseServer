using CSharpFunctionalExtensions;
using WarehouseServer.Domain.Entities;

namespace WarehouseServer.Domain.Interfaces.Services
{
    public interface IProductService
    {
        Task<Guid> AddProduct(Product product);
        Guid DeleteProduct(Product product);
        Task<Result<Product, string>> GetProductById(Guid id);
        Task<Result<Product, string>> GetProductByIdWithResources(Guid id);
        Task<Result<List<Product>, string>> GetProducts();
        Task<Result<List<Product>, string>> GetProductsWithResources();
        Guid SaveProduct(Product product);
    }
}