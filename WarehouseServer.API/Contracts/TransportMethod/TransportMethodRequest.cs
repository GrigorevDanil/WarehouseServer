namespace WarehouseServer.API.Contracts.TransportMethod
{
    public record TransportMethodRequest(
        Guid[] Warehouses,
        Guid[] Shops,
        Guid[] Products,
        double[] Demands
        );
}
