
using CSharpFunctionalExtensions;
using WarehouseServer.Domain.Entities;

namespace WarehouseServer.Domain.Interfaces.Services
{
    public interface IWarehouseService
    {
        Task<Guid> AddWarehouse(Warehouse warehouse);
        Guid DeleteWarehouse(Warehouse warehouse);
        Guid SaveWarehouse(Warehouse warehouse);
        Task<Result<Warehouse, string>> GetWarehouseById(Guid id);
        Task<Result<Warehouse, string>> GetWarehouseByIdWithProducts(Guid id);
        Task<Result<List<Warehouse>, string>> GetWarehouses();
        Task<Result<List<Warehouse>, string>> GetWarehousesWithProducts();
        Task<Result<List<Warehouse>, string>> GetWarehousesByIdsWithProducts(Guid[] ids);
    }
}
