using CSharpFunctionalExtensions;
using WarehouseServer.Domain.Entities;

namespace WarehouseServer.Domain.Interfaces.Repositories
{
    public interface IProductRepository
    {
        Task<Guid> Add(Product product);
        Guid Delete(Product product);
        Guid Save(Product product);
        Task<Result<Product, string>> GetById(Guid id);
        Task<Result<Product, string>> GetByIdWithResources(Guid id);
        Task<Result<List<Product>, string>> Get();
        Task<Result<List<Product>, string>> GetWithResources();
    }
}
