using AutoMapper;
using BLL.Services.Interfaces;
using DAL.Repositories.Interfaces;
using Domain.Entities;
using ViewModel.Category;

namespace BLL.Services.Implementation
{
    internal class CategoryService : GenericService<Category,
                                        CategoryViewModel,
                                        CategoryCreateModel,
                                        CategoryUpdateModel>, ICategoryService
    {
        public CategoryService(ICategoryRepository repository, IMapper mapper) : base(repository, mapper) { }

        public async Task UpdateImage(int imageId, byte[] image)
        {
            var category = await _repository.GetAsync(imageId).ConfigureAwait(false);
            if(category == null) {
                throw new Exception("Not found");
            }
            category.Picture = image;
            await _repository.UpdateAsync(category);
        }
    }
}
