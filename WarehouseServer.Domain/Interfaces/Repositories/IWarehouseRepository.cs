using CSharpFunctionalExtensions;
using WarehouseServer.Domain.Entities;

namespace WarehouseServer.Domain.Interfaces.Repositories
{
    public interface IWarehouseRepository
    {
        Task<Guid> Add(Warehouse warehouse);
        Guid Delete(Warehouse warehouse);
        Guid Save(Warehouse warehouse);
        Task<Result<Warehouse, string>> GetById(Guid id);
        Task<Result<Warehouse, string>> GetByIdWithProducts(Guid id);
        Task<Result<List<Warehouse>, string>> Get();
        Task<Result<List<Warehouse>, string>> GetWithProducts();
    }
}
