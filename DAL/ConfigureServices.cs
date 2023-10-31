using DAL.DBContext;
using DAL.Repositories.Implementaion;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DAL
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddDalDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            // database
            var connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            services.AddSingleton<INorthwindContext, NorthwindContext>(serviceProvider => new NorthwindContext(connectionString));

            // Repositories
            services.AddSingleton<ICategoryRepository, CategoryRepository>();
            services.AddSingleton<IProductRepository, ProductRepository>();
            services.AddSingleton<ISupplierRepository, SupplierRepository>();

            return services;
        }
    }
}
