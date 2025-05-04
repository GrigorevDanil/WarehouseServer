using CSharpFunctionalExtensions;

namespace WarehouseServer.Domain.Entities
{
    public class Warehouse : Entity<Guid>
    {
        public const int MAX_TITLE_LENGHT = 100;
        private List<ProductWarehouse> _productWarehouses = [];
        private List<Distance> _distances = [];

        private Warehouse()
        {

        }

        public Warehouse(string title, IEnumerable<ProductWarehouse> productWarehouses, IEnumerable<Distance> distances)
        {
            Title = title;
            _productWarehouses = productWarehouses.ToList();
            _distances = distances.ToList();
        }

        public static Result<Warehouse> Create(string title, IEnumerable<ProductWarehouse> productWarehouses, IEnumerable<Distance> distances)
        {
            if (string.IsNullOrEmpty(title) || title.Length > MAX_TITLE_LENGHT) return Result.Failure<Warehouse>($"`{nameof(title)}` не может быть пустым или превышать длину в {MAX_TITLE_LENGHT} символов");

            return Result.Success(new Warehouse(title, productWarehouses, distances));
        }


        public string Title { get; private set; } = string.Empty;
        public IReadOnlyList<ProductWarehouse> ProductWarehouses => _productWarehouses;
        public IReadOnlyList<Distance> Distances => _distances;

        public void UpdateInfo(string title)
        {
            Title = title;
        }

        public void AddProductWarehouse(ProductWarehouse productWarehouse)
        {
            _productWarehouses.Add(productWarehouse);
        }

        public void DeleteProductWarehouse(ProductWarehouse productWarehouse)
        {
            _productWarehouses.Remove(productWarehouse);
        }
    }
}
