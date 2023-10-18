 using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Homework.Entities.ViewModel.Category
{
    public class CategoryUpdateModel : CategoryBaseModel
    {
        public int CategoryId { get; set; }
        [AllowNull]
        public IFormFile? FormFile { get; set; }
    }
}
