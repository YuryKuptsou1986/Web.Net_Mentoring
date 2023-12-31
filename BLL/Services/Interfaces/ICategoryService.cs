﻿using ViewModel.Category;

namespace BLL.Services.Interfaces
{
    public interface ICategoryService : IGenericService<CategoryViewModel, CategoryCreateModel, CategoryUpdateModel>
    {
        Task UpdateImage(int imageId, byte[] image);
    }
}
