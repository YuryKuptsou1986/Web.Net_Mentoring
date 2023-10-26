using System.Diagnostics.CodeAnalysis;
using ViewModel.Category;

namespace Homework.Entities.ViewModel.Category
{
    public class CategoryPageUpdateModel : CategoryUpdateModel
    {
        [AllowNull]
        public IFormFile? FormFile { get; set; }
    }
}
