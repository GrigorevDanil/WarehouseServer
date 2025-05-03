using CSharpFunctionalExtensions;

namespace WarehouseServer.Domain.Entities
{
    public class ProductWarehouse : Entity<Guid>
    {
        private ProductWarehouse()
        {

        }

        private ProductWarehouse(Product product, Warehouse warehouse, int quantity)
        {
            ProductId = product.Id;
            WarehouseId = warehouse.Id;
            Product = product;
            Warehouse = warehouse;
            Quantity = quantity;
        }

        public static Result<ProductWarehouse> Create(Product product, Warehouse warehouse, int quantity)
        {
            return Result.Success(new ProductWarehouse(product, warehouse, quantity));
        }


        public Guid ProductId { get; private set; }
        public virtual Product? Product { get; }
        public Guid WarehouseId { get; }
        public virtual Warehouse? Warehouse { get; }
        public int Quantity { get; private set; }
    }
}
