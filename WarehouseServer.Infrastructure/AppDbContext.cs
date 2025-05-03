using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using WarehouseServer.Domain.Entities;
using WarehouseServer.Infrastructure.Configurations;

namespace WarehouseServer.Infrastructure
{
    public class AppDbContext(IConfiguration configuration) : DbContext
    {
        public DbSet<Distance> Distances => Set<Distance>();
        public DbSet<Product> Products => Set<Product>();
        public DbSet<Resource> Resources => Set<Resource>();
        public DbSet<ProductResource> ProductResources => Set<ProductResource>();
        public DbSet<ProductWarehouse> ProductWarehouses => Set<ProductWarehouse>();
        public DbSet<Shop> Shops => Set<Shop>();
        public DbSet<Warehouse> Warehouses => Set<Warehouse>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
            .UseMySql(
                    configuration.GetConnectionString(Constants.DATABASE),
                    ServerVersion.AutoDetect(configuration.GetConnectionString(Constants.DATABASE)));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new DistanceConfiguration());
            modelBuilder.ApplyConfiguration(new ProductConfiguration());
            modelBuilder.ApplyConfiguration(new ResourceConfiguration());
            modelBuilder.ApplyConfiguration(new ProductResourceConfiguration());
            modelBuilder.ApplyConfiguration(new ProductWarehouseConfiguration());
            modelBuilder.ApplyConfiguration(new ShopConfiguration());
            modelBuilder.ApplyConfiguration(new WarehouseConfiguration());
        }

    }
}
