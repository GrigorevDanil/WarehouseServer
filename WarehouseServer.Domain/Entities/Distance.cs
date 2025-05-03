using CSharpFunctionalExtensions;

namespace WarehouseServer.Domain.Entities
{
    public class Distance
    {

        private Distance()
        {

        }

        private Distance(Shop shop, Warehouse warehouse, int length)
        {
            ShopId = shop.Id;
            WarehouseId = warehouse.Id;
            Length = length;
        }

        public static Result<Distance> Create(Shop shop, Warehouse warehouse, int length)
        {
            return Result.Success(new Distance(shop, warehouse, length));
        }

        public Guid ShopId { get; }
        public virtual Shop? Shop { get; }
        public Guid WarehouseId { get; private set; }
        public virtual Warehouse? Warehouse { get; }
        public int Length { get; private set; }

    }
}
