using System.Text.Json.Serialization;
using ViewModel.Product;

namespace ViewModel.Supplier
{
    public class SupplierViewModel : SupplierBaseModel
    {
        public int SupplierId { get; set; }
        [JsonIgnore]
        public IEnumerable<ProductViewModel> Products { get; set; }
    }
}
