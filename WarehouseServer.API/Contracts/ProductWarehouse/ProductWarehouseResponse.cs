using WarehouseServer.API.Contracts.Product;

namespace WarehouseServer.API.Contracts.ProductWarehouse
{
    public record ProductWarehouseResponse(
       ProductResponse Product,
       int Quantity
       );
}
