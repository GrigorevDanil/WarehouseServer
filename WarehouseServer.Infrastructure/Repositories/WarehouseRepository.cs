using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using WarehouseServer.Domain.Entities;
using WarehouseServer.Domain.Interfaces.Repositories;

namespace WarehouseServer.Infrastructure.Repositories
{
    public class WarehouseRepository : IWarehouseRepository
    {
        private readonly AppDbContext dbContext;

        public WarehouseRepository(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Guid> Add(Warehouse warehouse)
        {
            await dbContext.Warehouses.AddAsync(warehouse);
            return warehouse.Id;
        }

        public Guid Delete(Warehouse warehouse)
        {
            dbContext.Warehouses.Remove(warehouse);
            return warehouse.Id;
        }

        public async Task<Result<List<Warehouse>, string>> Get()
        {
            var warehouses = await dbContext.Warehouses.ToListAsync();

            if (warehouses is null)
                return Result.Failure<List<Warehouse>, string>("Склады не найдены");

            return warehouses;
        }

        public async Task<Result<Warehouse, string>> GetById(Guid id)
        {
            var warehouse = await dbContext.Warehouses.FirstOrDefaultAsync(w => w.Id == id);

            if (warehouse is null)
                return Result.Failure<Warehouse, string>("Склад не найден");

            return warehouse;
        }

        public async Task<Result<Warehouse, string>> GetByIdWithProducts(Guid id)
        {
            var warehouse = await dbContext.Warehouses.Include(e => e.ProductWarehouses).ThenInclude(pw => pw.Product).FirstOrDefaultAsync(w => w.Id == id);

            if (warehouse is null)
                return Result.Failure<Warehouse, string>("Склад не найден");

            return warehouse;
        }

        public async Task<Result<List<Warehouse>, string>> GetWithProducts()
        {
            var warehouses = await dbContext.Warehouses.Include(e => e.ProductWarehouses).ThenInclude(pw => pw.Product).ToListAsync();

            if (warehouses is null)
                return Result.Failure<List<Warehouse>, string>("Склады не найдены");

            return warehouses;
        }

        public Guid Save(Warehouse warehouse)
        {
            dbContext.Warehouses.Attach(warehouse);
            return warehouse.Id;
        }
    }
}
