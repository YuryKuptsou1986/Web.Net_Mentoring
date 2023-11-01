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
            services.AddTransient<ICategoryService, CategoryService>();
            services.AddTransient<IProductService, ProductService>();
            services.AddTransient<ISupplierService, SupplierService>();
            services.AddSingleton<INorthwindImageConverterService, NorthwindImageConverterService>();

            services.AddAutoMapper(typeof(AppMappingProfile));

            return services;
        }
    }
}
