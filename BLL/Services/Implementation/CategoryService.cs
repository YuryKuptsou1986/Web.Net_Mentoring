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
        private readonly INorthwindImageConverterService _imageConverterService;

        public CategoryService(ICategoryRepository repository,
            IMapper mapper,
            INorthwindImageConverterService imageConverterService) : base(repository, mapper)
        {
            _imageConverterService = imageConverterService;
        }

        public async Task UpdateImage(int imageId, byte[] image)
        {
            var category = await _repository.GetAsync(imageId).ConfigureAwait(false);
            if(category == null) {
                throw new Exception("Not found");
            }
            category.Picture = _imageConverterService.ConvertToNorthwindImage(image);
            await _repository.UpdateAsync(category);
        }

        public override async Task<int> AddAsync(CategoryCreateModel entity)
        {
            var originalPicture = entity.Picture;
            entity.Picture = _imageConverterService.ConvertToNorthwindImage(originalPicture);
            var createEntity = _mapper.Map<Category>(entity);
            await _repository.AddAsync(createEntity);
            entity.Picture = originalPicture;
            return createEntity.CategoryId;
        }

        public override async Task<IEnumerable<CategoryViewModel>> GetAllAsync()
        {
            var entities = await base.GetAllAsync();
            foreach (var item in entities)
            {
                item.Picture = _imageConverterService.ConvertToNormalImage(item.Picture);
            }
            return entities;
        }

        public override async Task<CategoryViewModel> GetAsync(int id)
        {
            var entity = await base.GetAsync(id);
            entity.Picture = _imageConverterService.ConvertToNormalImage(entity.Picture);
            return entity;
        }

        public override async Task<CategoryViewModel> GetAsync(string id)
        {
            var entity = await base.GetAsync(id);
            entity.Picture = _imageConverterService.ConvertToNormalImage(entity.Picture);
            return entity;
        }

        public override async Task<CategoryViewModel> GetAsync(Guid id)
        {
            var entity = await base.GetAsync(id);
            entity.Picture = _imageConverterService.ConvertToNormalImage(entity.Picture);
            return entity;
        }

        public virtual async Task<IEnumerable<CategoryViewModel>> GetTopAsync(int top)
        {
            var entities = await base.GetTopAsync(top);
            foreach (var item in entities) {
                item.Picture = _imageConverterService.ConvertToNormalImage(item.Picture);
            }
            return entities;
        }

        public virtual async Task UpdateAsync(CategoryUpdateModel entity)
        {
            var originalPicture = entity.Picture;
            entity.Picture = _imageConverterService.ConvertToNorthwindImage(originalPicture);
            await base.UpdateAsync(entity);
            entity.Picture = originalPicture;
        }
    }
}
