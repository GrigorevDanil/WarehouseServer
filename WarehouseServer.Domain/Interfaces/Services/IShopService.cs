
using CSharpFunctionalExtensions;
using WarehouseServer.Domain.Entities;

namespace WarehouseServer.Domain.Interfaces.Services
{
    public interface IShopService
    {
        Task<Guid> AddShop(Shop shop);
        Guid DeleteShop(Shop shop);
        Guid SaveShop(Shop shop);
        Task<Result<Shop, string>> GetShopById(Guid id);
        Task<Result<Shop, string>> GetShopByIdWithDistances(Guid id);
        Task<Result<List<Shop>, string>> GetShops();
        Task<Result<List<Shop>, string>> GetShopsWithDistances();
    }
}
