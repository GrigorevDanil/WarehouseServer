
namespace WarehouseServer.Application.Interfaces
{
    public interface ITransportMethodService
    {
        double Calculate(double[,] A, double[] suppliers, double[] demands);
    }
}
