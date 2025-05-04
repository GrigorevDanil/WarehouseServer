using Microsoft.Extensions.DependencyInjection;
using WarehouseServer.Application.Services.Repositories;
using WarehouseServer.Domain.Interfaces.Services;

namespace WarehouseServer.Application
{
    public static class Registration
    {
        public static IServiceCollection AddApplication(
        this IServiceCollection services)
        {
            services
                .AddServices();

            return services;
        }

        private static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IResourceService, ResourceService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IWarehouseService, WarehouseService>();
            services.AddScoped<IShopService, ShopService>();

            return services;
        }
    }
}
