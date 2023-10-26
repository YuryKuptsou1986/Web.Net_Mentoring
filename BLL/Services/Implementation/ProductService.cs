using AutoMapper;
using BLL.Services.Interfaces;
using DAL.Repositories.Interfaces;
using Domain.Entities;
using ViewModel.Product;

namespace BLL.Services.Implementation
{
    internal class ProductService : GenericService<Product,
                                        ProductViewModel,
                                        ProductCreateModel,
                                        ProductUpdateModel>, IProductService
    {
        public ProductService(IProductRepository repository, IMapper mapper) : base(repository, mapper) { }
    }
}
