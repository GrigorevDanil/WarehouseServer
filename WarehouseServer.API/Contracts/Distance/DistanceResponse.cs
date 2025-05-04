using WarehouseServer.API.Contracts.Warehouse;

namespace WarehouseServer.API.Contracts.Distance
{
    public record DistanceResponse(
        WarehouseResponse Warehouse,
        int Length
        );
}
