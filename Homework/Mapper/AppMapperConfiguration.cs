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

            // Product
            CreateMap<Product, ProductViewModel>();
            CreateMap<ProductCreateModel, Product>();
            CreateMap<ProductUpdateModel, Product>();
            CreateMap<ProductUpdateModel, ProductViewModel>().ReverseMap();

            // Supplier
            CreateMap<Supplier, SupplierViewModel>();
            CreateMap<SupplierCreateModel, Supplier>();
            CreateMap<SupplierUpdateModel, Supplier>();
        }
    }
}
