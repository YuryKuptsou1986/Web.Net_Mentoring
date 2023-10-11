using Homework.Entities.ViewModel.Product;

namespace Homework.Entities.ViewModel.Supplier
{
    public class SupplierViewModel : SupplierBaseModel
    {
        public int SupplierId { get; set; }
        public IEnumerable<ProductViewModel> Products { get; set; }
    }
}
