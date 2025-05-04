using CSharpFunctionalExtensions;

namespace WarehouseServer.Domain.Entities
{
    public class Resource : Entity<Guid>
    {
        public const int MAX_TITLE_LENGHT = 100;
        public const int MAX_UNIT_LENGHT = 10;
        private List<ProductResource> _productResources = [];

        private Resource()
        {
        }

        private Resource(string title, string unit, IEnumerable<ProductResource> productResources)
        {
            Title = title;
            Unit = unit;
            _productResources = productResources.ToList();
        }

        public static Result<Resource> Create(string title, string unit, IEnumerable<ProductResource> productResources)
        {
            if (string.IsNullOrEmpty(title) || title.Length > MAX_TITLE_LENGHT) return Result.Failure<Resource>($"`{nameof(title)}` не может быть пустым или превышать длину в {MAX_TITLE_LENGHT} символов");
            if (string.IsNullOrEmpty(unit) || unit.Length > MAX_UNIT_LENGHT) return Result.Failure<Resource>($"`{nameof(unit)}` не может быть пустым или превышать длину в {MAX_UNIT_LENGHT} символов");

            return Result.Success(new Resource(title, unit, productResources));
        }


        public string Title { get; private set; } = string.Empty;
        public string Unit { get; private set; } = string.Empty;
        public IReadOnlyList<ProductResource> ProductResources => _productResources;

        public void UpdateInfo(string title, string unit)
        {
            Title = title;
            Unit = unit;
        }

    }
}
