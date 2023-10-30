using System.Text.Json.Serialization;
using ViewModel.Category;
using ViewModel.OrderDetail;
using ViewModel.Supplier;

namespace ViewModel.Product
{
    public class ProductViewModel : ProductBaseModel
    {
        public int ProductId { get; set; }

        public CategoryViewModel Category { get; set; }
        public SupplierViewModel Supplier { get; set; }
        [JsonIgnore]
        public IEnumerable<OrderDetailViewModel> OrderDetails { get; set; }
    }
}
