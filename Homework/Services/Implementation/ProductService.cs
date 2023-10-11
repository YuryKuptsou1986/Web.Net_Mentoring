using AutoMapper;
using Homework.DBContext.Repositories.Interfaces;
using Homework.Entities.Data;
using Homework.Entities.ViewModel.Product;
using Homework.Services.Interfaces;

namespace Homework.Services.Implementation
{
    public class ProductService : GenericService<Product,
                                        ProductViewModel,
                                        ProductCreateModel,
                                        ProductUpdateModel>, IProductService
    {
        public ProductService(IProductRepository repository, IMapper mapper) : base(repository, mapper) { }
    }
}
