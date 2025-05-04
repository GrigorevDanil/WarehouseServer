using CSharpFunctionalExtensions;
using WarehouseServer.Domain.Entities;

namespace WarehouseServer.Domain.Interfaces.Repositories
{
    public interface IResourceRepository
    {
        Task<Guid> Add(Resource resource);
        Guid Delete(Resource resource);
        Guid Save(Resource resource);
        Task<Result<Resource, string>> GetById(Guid id);
        Task<Result<List<Resource>, string>> Get();
    }
}
