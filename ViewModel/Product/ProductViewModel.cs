using System.Text.Json.Serialization;
using ViewModel.Category;
using ViewModel.OrderDetail;
using ViewModel.Supplier;

namespace ViewModel.Product
{
    public class ProductViewModel : ProductBaseModel
    {
        public int ProductId { get; set; }

        [JsonIgnore]
        public CategoryViewModel Category { get; set; }
        [JsonIgnore]
        public SupplierViewModel Supplier { get; set; }
        [JsonIgnore]
        public IEnumerable<OrderDetailViewModel> OrderDetails { get; set; }
    }
}
