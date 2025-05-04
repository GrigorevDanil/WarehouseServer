namespace WarehouseServer.API.Contracts.Resource
{
    public record ResourceResponse(
        Guid Id,
        string Title,
        string Unit
        );
}
