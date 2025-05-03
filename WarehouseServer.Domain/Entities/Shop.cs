using CSharpFunctionalExtensions;

namespace WarehouseServer.Domain.Entities
{
    public class Shop : Entity<Guid>
    {
        public const int MAX_TITLE_LENGHT = 100;
        private readonly List<Distance> _distances = [];

        private Shop()
        {

        }

        public Shop(string title, IEnumerable<Distance> distances)
        {
            Title = title;
            _distances = distances.ToList();
        }

        public static Result<Shop> Create(string title, IEnumerable<Distance> distances)
        {
            if (string.IsNullOrEmpty(title) || title.Length > MAX_TITLE_LENGHT) return Result.Failure<Shop>($"`{nameof(title)}` не может быть пустым или превышать длину в {MAX_TITLE_LENGHT} символов");

            return Result.Success(new Shop(title, distances));
        }


        public string Title { get; private set; } = string.Empty;
        public IReadOnlyList<Distance> Distances => _distances;
    }
}
