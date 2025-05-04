using WarehouseServer.Application.Interfaces;

namespace WarehouseServer.Infrastructure;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext dbContext;

    public UnitOfWork(AppDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task SaveChanges()
    {
        await dbContext.SaveChangesAsync();
    }
}