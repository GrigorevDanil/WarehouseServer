using CSharpFunctionalExtensions;

namespace WarehouseServer.Domain.Entities
{
    public class Product : Entity<Guid>
    {
        public const int MAX_TITLE_LENGHT = 100;
        private readonly List<ProductResource> _productResources = [];
        private readonly List<ProductWarehouse> _productWarehouses = [];


        private Product()
        {
        }

        private Product(string title, float cost, IEnumerable<ProductResource> productResources, IEnumerable<ProductWarehouse> productWarehouses)
        {
            Title = title;
            Cost = cost;
            _productResources = productResources.ToList();
            _productWarehouses = productWarehouses.ToList();
        }

        public static Result<Product> Create(string title, float cost, IEnumerable<ProductResource> productResources, IEnumerable<ProductWarehouse> productWarehouses)
        {
            if (string.IsNullOrEmpty(title) || title.Length > MAX_TITLE_LENGHT) return Result.Failure<Product>($"`{nameof(title)}` не может быть пустым или превышать длину в {MAX_TITLE_LENGHT} символов");

            return Result.Success(new Product(title, cost, productResources, productWarehouses));
        }

        public string Title { get; private set; } = string.Empty;
        public float Cost { get; private set; }
        public IReadOnlyList<ProductResource> ProductResources => _productResources;
        public IReadOnlyList<ProductWarehouse> ProductWarehouses => _productWarehouses;

        public void UpdateInfo(string title, float cost)
        {
            Title = title;
            Cost = cost;
        }

        public void AddProductResource(ProductResource pr)
        {
            _productResources.Add(pr);
        }

        public void DeleteProductResource(ProductResource pr)
        {
            _productResources.Remove(pr);
        }
    }
}
