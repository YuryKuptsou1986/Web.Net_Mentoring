using BLL.Mappings;
using BLL.Services.Implementation;
using BLL.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace BLL
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddBllDependencies(this IServiceCollection services)
        {
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ISupplierService, SupplierService>();

            services.AddAutoMapper(typeof(AppMappingProfile));

            return services;
        }
    }
}
