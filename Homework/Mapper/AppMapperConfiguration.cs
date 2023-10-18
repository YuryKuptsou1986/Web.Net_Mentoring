using AutoMapper;
using Homework.Entities.Data;
using Homework.Entities.ViewModel.Category;
using Homework.Entities.ViewModel.Product;
using Homework.Entities.ViewModel.Supplier;

namespace Homework.Mapper
{
    public class AppMapperConfiguration : Profile
    {
        public AppMapperConfiguration()
        {
            // Category
            CreateMap<Category, CategoryViewModel>();
            CreateMap<CategoryCreateModel, Category>();
            CreateMap<CategoryUpdateModel, Category>();
            CreateMap<CategoryCreateModel, CategoryViewModel>().ReverseMap();
            CreateMap<CategoryUpdateModel, CategoryViewModel>().ReverseMap();

            // Product
            CreateMap<Product, ProductViewModel>();
            CreateMap<ProductCreateModel, Product>();
            CreateMap<ProductUpdateModel, Product>();
            CreateMap<ProductUpdateModel, ProductViewModel>().ReverseMap();
            CreateMap<ProductCreateModel, ProductViewModel>().ReverseMap();

            // Supplier
            CreateMap<Supplier, SupplierViewModel>();
            CreateMap<SupplierCreateModel, Supplier>();
            CreateMap<SupplierUpdateModel, Supplier>();
            CreateMap<SupplierCreateModel, SupplierViewModel>().ReverseMap();
        }
    }
}
