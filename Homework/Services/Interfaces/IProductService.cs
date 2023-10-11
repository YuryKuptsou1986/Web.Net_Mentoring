using Homework.Entities.ViewModel.Product;

namespace Homework.Services.Interfaces
{
    public interface IProductService : IGenericService<ProductViewModel, ProductCreateModel, ProductUpdateModel>
    {
    }
}
