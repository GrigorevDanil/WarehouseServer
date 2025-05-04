using Microsoft.Extensions.DependencyInjection;
using WarehouseServer.Application.Interfaces;
using WarehouseServer.Domain.Interfaces.Repositories;
using WarehouseServer.Infrastructure.Repositories;
using WarehouseServer.Infrastructure.Services;

namespace WarehouseServer.Infrastructure
{
    public static class Registration
    {
        public static IServiceCollection AddInfrastructure(
        this IServiceCollection services)
        {
            services
                .AddScoped<AppDbContext>()
                .AddRepositories()
                .AddServices();

            return services;
        }

        private static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IResourceRepository, ResourceRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IWarehouseRepository, WarehouseRepository>();
            services.AddScoped<IShopRepository, ShopRepository>();

            return services;
        }
        private static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ITransportMethodService, TransportMethodService>();

            return services;
        }
    }
}
