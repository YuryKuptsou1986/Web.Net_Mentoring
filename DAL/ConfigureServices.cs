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
            services.AddDbContext<NorthwindContext>(options =>
                    options.UseSqlServer(connectionString));

            // Repositories
            services.AddScoped<INorthwindContext, NorthwindContext>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ISupplierRepository, SupplierRepository>();

            return services;
        }
    }
}
