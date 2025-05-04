using WarehouseServer.API.Contracts.Resource;

namespace WarehouseServer.API.Contracts.ProductResource
{
    public record ProductResourceResponse(
       ResourceResponse Resource,
       int Quantity
       );
}
