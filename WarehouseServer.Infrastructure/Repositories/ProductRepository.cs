using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using WarehouseServer.Domain.Entities;
using WarehouseServer.Domain.Interfaces.Repositories;

namespace WarehouseServer.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext dbContext;

        public ProductRepository(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Guid> Add(Product product)
        {
            await dbContext.Products.AddAsync(product);
            return product.Id;
        }

        public Guid Delete(Product product)
        {
            dbContext.Products.Remove(product);
            return product.Id;
        }

        public async Task<Result<List<Product>, string>> Get()
        {
            var products = await dbContext.Products.ToListAsync();

            if (products is null)
                return Result.Failure<List<Product>, string>("Товары не найден");

            return products;
        }

        public async Task<Result<Product, string>> GetById(Guid id)
        {
            var product = await dbContext.Products.FirstOrDefaultAsync(p => p.Id == id);

            if (product is null)
                return Result.Failure<Product, string>("Товар не найден");

            return product;
        }

        public async Task<Result<Product, string>> GetByIdWithResources(Guid id)
        {
            var product = await dbContext.Products.Include(p => p.ProductResources).ThenInclude(pr => pr.Resource).FirstOrDefaultAsync(p => p.Id == id);

            if (product is null)
                return Result.Failure<Product, string>("Товар не найден");

            return product;
        }

        public async Task<Result<List<Product>, string>> GetWithResources()
        {
            var products = await dbContext.Products.Include(p => p.ProductResources).ThenInclude(pr => pr.Resource).ToListAsync();

            if (products is null)
                return Result.Failure<List<Product>, string>("Товары не найден");

            return products;
        }

        public Guid Save(Product product)
        {
            dbContext.Products.Attach(product);
            return product.Id;
        }
    }
}
