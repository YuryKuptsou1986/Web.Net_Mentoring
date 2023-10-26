using ViewModel.Product;

namespace BLL.Services.Interfaces
{
    public interface IProductService : IGenericService<ProductViewModel, ProductCreateModel, ProductUpdateModel>
    {
    }
}
