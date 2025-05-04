using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using WarehouseServer.Domain.Entities;
using WarehouseServer.Domain.Interfaces.Repositories;

namespace WarehouseServer.Infrastructure.Repositories
{
    public class ResourceRepository : IResourceRepository
    {
        private readonly AppDbContext dbContext;

        public ResourceRepository(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Guid> Add(Resource resource)
        {
            await dbContext.Resources.AddAsync(resource);
            return resource.Id;
        }

        public Guid Delete(Resource resource)
        {
            dbContext.Resources.Remove(resource);
            return resource.Id;
        }

        public async Task<Result<List<Resource>, string>> Get()
        {
            var resources = await dbContext.Resources.ToListAsync();

            if (resources is null)
                return Result.Failure<List<Resource>, string>("Ресурсы не найден");

            return resources;
        }

        public async Task<Result<Resource, string>> GetById(Guid id)
        {
            var resource = await dbContext.Resources.FirstOrDefaultAsync(r => r.Id == id);

            if (resource is null)
                return Result.Failure<Resource, string>("Ресурс не найден");

            return resource;
        }

        public Guid Save(Resource resource)
        {
            dbContext.Resources.Attach(resource);
            return resource.Id;
        }
    }
}
