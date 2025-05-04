namespace WarehouseServer.Application.Interfaces
{
    public interface IUnitOfWork
    {
        Task SaveChanges();
    }
}
