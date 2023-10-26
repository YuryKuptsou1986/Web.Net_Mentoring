using ViewModel.Product;

namespace ViewModel.Category
{
    public class CategoryViewModel : CategoryBaseModel
    {
        public int CategoryId { get; set; }
        public IEnumerable<ProductViewModel> Products { get; set; }
    }
}
