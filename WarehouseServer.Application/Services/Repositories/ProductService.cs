using CSharpFunctionalExtensions;
using WarehouseServer.Domain.Entities;
using WarehouseServer.Domain.Interfaces.Repositories;
using WarehouseServer.Domain.Interfaces.Services;

namespace WarehouseServer.Application.Services.Repositories
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository repository;

        public ProductService(IProductRepository repository)
        {
            this.repository = repository;
        }

        public Task<Guid> AddProduct(Product product) => repository.Add(product);
        public Guid DeleteProduct(Product product) => repository.Delete(product);
        public Task<Result<Product, string>> GetProductById(Guid id) => repository.GetById(id);

        public Task<Result<Product, string>> GetProductByIdWithResources(Guid id) => repository.GetByIdWithResources(id);

        public Task<Result<List<Product>, string>> GetProducts() => repository.Get();
        public Task<Result<List<Product>, string>> GetProductsWithResources() => repository.GetWithResources();
        public Guid SaveProduct(Product product) => repository.Save(product);
    }
}
