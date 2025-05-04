using CSharpFunctionalExtensions;
using WarehouseServer.Domain.Entities;
using WarehouseServer.Domain.Interfaces.Repositories;
using WarehouseServer.Domain.Interfaces.Services;

namespace WarehouseServer.Application.Services.Repositories
{
    public class WarehouseService : IWarehouseService
    {
        private readonly IWarehouseRepository repository;

        public WarehouseService(IWarehouseRepository repository)
        {
            this.repository = repository;
        }

        public Task<Guid> AddWarehouse(Warehouse warehouse) => repository.Add(warehouse);

        public Guid DeleteWarehouse(Warehouse warehouse) => repository.Delete(warehouse);

        public Task<Result<Warehouse, string>> GetWarehouseById(Guid id) => repository.GetById(id);

        public Task<Result<Warehouse, string>> GetWarehouseByIdWithProducts(Guid id) => repository.GetByIdWithProducts(id);

        public Task<Result<List<Warehouse>, string>> GetWarehouses() => repository.Get();

        public Task<Result<List<Warehouse>, string>> GetWarehousesWithProducts() => repository.GetWithProducts();

        public Guid SaveWarehouse(Warehouse warehouse) => repository.Save(warehouse);
    }
}
