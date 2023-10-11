using AutoMapper;
using Homework.DBContext.Repositories.Interfaces;
using Homework.Entities.Data;
using Homework.Entities.ViewModel.Category;
using Homework.Services.Interfaces;

namespace Homework.Services.Implementation
{
    public class CategoryService : GenericService<Category,
                                        CategoryViewModel,
                                        CategoryCreateModel,
                                        CategoryUpdateModel>, ICategoryService
    {
        public CategoryService(ICategoryRepository repository, IMapper mapper) : base(repository, mapper) { }
    }
}
