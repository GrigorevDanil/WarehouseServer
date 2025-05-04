using CSharpFunctionalExtensions;
using WarehouseServer.Domain.Entities;

namespace WarehouseServer.Domain.Interfaces.Repositories
{
    public interface IShopRepository
    {
        Task<Guid> Add(Shop shop);
        Guid Delete(Shop shop);
        Guid Save(Shop shop);
        Task<Result<Shop, string>> GetById(Guid id);
        Task<Result<Shop, string>> GetByIdWithDistances(Guid id);
        Task<Result<List<Shop>, string>> Get();
        Task<Result<List<Shop>, string>> GetWithDistances();
        Task<Result<List<Shop>, string>> GetByIdsWithDistances(Guid[] ids);
    }
}
