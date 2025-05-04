using CSharpFunctionalExtensions;
using WarehouseServer.Domain.Entities;
using WarehouseServer.Domain.Interfaces.Repositories;
using WarehouseServer.Domain.Interfaces.Services;

namespace WarehouseServer.Application.Services.Repositories
{
    public class ResourceService : IResourceService
    {
        private readonly IResourceRepository repository;

        public ResourceService(IResourceRepository repository)
        {
            this.repository = repository;
        }

        public Task<Guid> AddResource(Resource resource) => repository.Add(resource);
        public Guid DeleteResource(Resource resource) => repository.Delete(resource);
        public Task<Result<Resource, string>> GetResourceById(Guid id) => repository.GetById(id);
        public Task<Result<List<Resource>, string>> GetResources() => repository.Get();
        public Guid SaveResource(Resource resource) => repository.Save(resource);
    }
}
