using CSharpFunctionalExtensions;
using WarehouseServer.Domain.Entities;

namespace WarehouseServer.Domain.Interfaces.Services
{
    public interface IResourceService
    {
        Task<Guid> AddResource(Resource resource);
        Guid DeleteResource(Resource resource);
        Task<Result<Resource, string>> GetResourceById(Guid id);
        Guid SaveResource(Resource resource);
        Task<Result<List<Resource>, string>> GetResources();
    }
}