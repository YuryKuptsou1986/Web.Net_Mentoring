using Homework.Entities.ViewModel.Category;

namespace Homework.Services.Interfaces
{
    public interface ICategoryService : IGenericService<CategoryViewModel, CategoryCreateModel, CategoryUpdateModel>
    {
    }
}
