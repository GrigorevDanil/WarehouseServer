using WarehouseServer.API.Contracts.ProductResource;

namespace WarehouseServer.API.Contracts.Product
{
    public record ProductResponse(
        Guid Id,
        string Title,
        float Cost,
        ProductResourceResponse[] Resources
        );


}
