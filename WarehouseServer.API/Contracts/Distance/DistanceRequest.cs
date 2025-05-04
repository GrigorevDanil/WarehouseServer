namespace WarehouseServer.API.Contracts.Distance
{
    public record DistanceRequest(
       Guid WarehouseId,
       int Length
       );

    public record UpdateDistanceRequest(
       int Length
       );
}
