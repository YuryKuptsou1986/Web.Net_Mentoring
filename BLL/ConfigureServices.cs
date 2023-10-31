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
            services.AddSingleton<ICategoryService, CategoryService>();
            services.AddSingleton<IProductService, ProductService>();
            services.AddSingleton<ISupplierService, SupplierService>();

            services.AddAutoMapper(typeof(AppMappingProfile));

            return services;
        }
    }
}
