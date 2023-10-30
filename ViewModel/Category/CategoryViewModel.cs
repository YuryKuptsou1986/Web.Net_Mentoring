using System.Text.Json.Serialization;
using ViewModel.Product;

namespace ViewModel.Category
{
    public class CategoryViewModel : CategoryBaseModel
    {
        public int CategoryId { get; set; }
        [JsonIgnore]
        public IEnumerable<ProductViewModel> Products { get; set; }
    }
}
