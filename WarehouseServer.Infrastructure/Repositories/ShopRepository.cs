using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using WarehouseServer.Domain.Entities;
using WarehouseServer.Domain.Interfaces.Repositories;

namespace WarehouseServer.Infrastructure.Repositories
{
    public class ShopRepository : IShopRepository
    {
        private readonly AppDbContext dbContext;

        public ShopRepository(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Guid> Add(Shop shop)
        {
            await dbContext.Shops.AddAsync(shop);
            return shop.Id;
        }

        public Guid Delete(Shop shop)
        {
            dbContext.Shops.Remove(shop);
            return shop.Id;
        }

        public async Task<Result<List<Shop>, string>> Get()
        {
            var shops = await dbContext.Shops.ToListAsync();

            if (shops is null)
                return Result.Failure<List<Shop>, string>("Магазины не найден");

            return shops;
        }

        public async Task<Result<Shop, string>> GetById(Guid id)
        {
            var shop = await dbContext.Shops.FirstOrDefaultAsync(s => s.Id == id);

            if (shop is null)
                return Result.Failure<Shop, string>("Магазин не найден");

            return shop;
        }

        public async Task<Result<List<Shop>, string>> GetByIdsWithDistances(Guid[] ids)
        {
            var shops = await dbContext.Shops.Include(e => e.Distances).ThenInclude(d => d.Warehouse).Where(s => ids.Contains(s.Id)).ToListAsync();

            if (shops is null || shops.Count == 0)
                return Result.Failure<List<Shop>, string>("Магазины не найдены");

            return shops;
        }

        public async Task<Result<Shop, string>> GetByIdWithDistances(Guid id)
        {
            var shop = await dbContext.Shops.Include(e => e.Distances).ThenInclude(d => d.Warehouse).FirstOrDefaultAsync(s => s.Id == id);

            if (shop is null)
                return Result.Failure<Shop, string>("Магазин не найден");

            return shop;
        }

        public async Task<Result<List<Shop>, string>> GetWithDistances()
        {
            var shops = await dbContext.Shops.Include(e => e.Distances).ThenInclude(d => d.Warehouse).ToListAsync();

            if (shops is null)
                return Result.Failure<List<Shop>, string>("Магазины не найден");

            return shops;
        }

        public Guid Save(Shop shop)
        {
            dbContext.Shops.Attach(shop);
            return shop.Id;
        }
    }
}
