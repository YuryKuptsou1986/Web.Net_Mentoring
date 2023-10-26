using AutoMapper;
using Domain.Entities;
using ViewModel.Category;
using ViewModel.Product;
using ViewModel.Supplier;

namespace BLL.Mappings
{
    public class AppMappingProfile : Profile
    {
        public AppMappingProfile()
        {
            // Category
            CreateMap<Category, CategoryViewModel>();
            CreateMap<CategoryCreateModel, Category>();
            CreateMap<CategoryUpdateModel, Category>();

            // Product
            CreateMap<Product, ProductViewModel>();
            CreateMap<ProductCreateModel, Product>();
            CreateMap<ProductUpdateModel, Product>();

            // Supplier
            CreateMap<Supplier, SupplierViewModel>();
            CreateMap<SupplierCreateModel, Supplier>();
            CreateMap<SupplierUpdateModel, Supplier>();
        }
    }
}
