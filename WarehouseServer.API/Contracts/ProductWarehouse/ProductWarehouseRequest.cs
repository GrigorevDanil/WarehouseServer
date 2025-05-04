namespace WarehouseServer.API.Contracts.ProductWarehouse
{
    public record ProductWarehouseRequest(
       Guid ProductId,
       int Quantity
       );

    public record UpdateProductWarehouseRequest(
       int Quantity
       );
}
