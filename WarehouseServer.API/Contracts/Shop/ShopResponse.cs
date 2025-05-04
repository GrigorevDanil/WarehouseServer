using WarehouseServer.API.Contracts.Distance;


namespace WarehouseServer.API.Contracts.Shop
{
    public record ShopResponse(
        Guid Id,
        string Title,
        DistanceResponse[] Distances);
}
