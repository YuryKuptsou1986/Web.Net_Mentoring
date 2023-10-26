using AutoMapper;
using Homework.Entities.ViewModel.Category;
using ViewModel.Category;
using ViewModel.Product;
using ViewModel.Supplier;

namespace Homework.Mapper
{
    public class AppMappingProfile : Profile
    {
        public AppMappingProfile()
        {
            CreateMap<CategoryCreateModel, CategoryViewModel>().ReverseMap();
            CreateMap<CategoryUpdateModel, CategoryViewModel>().ReverseMap();
            CreateMap<CategoryPageUpdateModel, CategoryViewModel>().ReverseMap();
            CreateMap<CategoryPageUpdateModel, CategoryUpdateModel>().ReverseMap();

            CreateMap<ProductUpdateModel, ProductViewModel>().ReverseMap();
            CreateMap<ProductCreateModel, ProductViewModel>().ReverseMap();

            CreateMap<SupplierCreateModel, SupplierViewModel>().ReverseMap();
        }
    }
}
