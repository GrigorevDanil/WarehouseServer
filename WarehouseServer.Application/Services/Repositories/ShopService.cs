using CSharpFunctionalExtensions;
using WarehouseServer.Domain.Entities;
using WarehouseServer.Domain.Interfaces.Repositories;
using WarehouseServer.Domain.Interfaces.Services;

namespace WarehouseServer.Application.Services.Repositories
{
    public class ShopService : IShopService
    {
        private readonly IShopRepository repository;

        public ShopService(IShopRepository repository)
        {
            this.repository = repository;
        }

        public Task<Guid> AddShop(Shop shop) => repository.Add(shop);

        public Guid DeleteShop(Shop shop) => repository.Delete(shop);

        public Task<Result<Shop, string>> GetShopById(Guid id) => repository.GetById(id);

        public Task<Result<Shop, string>> GetShopByIdWithDistances(Guid id) => repository.GetByIdWithDistances(id);

        public Task<Result<List<Shop>, string>> GetShops() => repository.Get();

        public Task<Result<List<Shop>, string>> GetShopsByIdsWithDistances(Guid[] ids) => repository.GetByIdsWithDistances(ids);

        public Task<Result<List<Shop>, string>> GetShopsWithDistances() => repository.GetWithDistances();

        public Guid SaveShop(Shop shop) => repository.Save(shop);
    }
}
