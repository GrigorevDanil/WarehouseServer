using CSharpFunctionalExtensions;

namespace WarehouseServer.Domain.Entities
{
    public class ProductResource
    {


        private ProductResource()
        {

        }

        private ProductResource(Product product, Resource resource, int quantity)
        {
            ProductId = product.Id;
            Product = product;
            ResourceId = resource.Id;
            Resource = resource;
            Quantity = quantity;
        }

        public static Result<ProductResource> Create(Product product, Resource resource, int quantity)
        {
            return Result.Success(new ProductResource(product, resource, quantity));
        }


        public Guid ProductId { get; }
        public virtual Product? Product { get; }
        public Guid ResourceId { get; private set; }
        public virtual Resource? Resource { get; }
        public int Quantity { get; private set; }

    }
}
