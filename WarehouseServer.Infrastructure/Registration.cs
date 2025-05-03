using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace WarehouseServer.Infrastructure
{
    public static class Registration
    {
        public static IServiceCollection AddInfrastructure(
        this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<AppDbContext>();

            return services;
        }
    }
}
