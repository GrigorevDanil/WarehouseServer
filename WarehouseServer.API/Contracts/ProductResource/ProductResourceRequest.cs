namespace WarehouseServer.API.Contracts.ProductResource
{
    public record ProductResourceRequest(
       Guid ResourceId,
       int Quantity
       );

    public record UpdateProductResourceRequest(
       int Quantity
       );
}
