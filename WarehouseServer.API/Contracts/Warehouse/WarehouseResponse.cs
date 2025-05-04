using WarehouseServer.API.Contracts.ProductWarehouse;

namespace WarehouseServer.API.Contracts.Warehouse
{
    public record WarehouseResponse(
        Guid Id,
        string Title,
        ProductWarehouseResponse[] Products);
}
