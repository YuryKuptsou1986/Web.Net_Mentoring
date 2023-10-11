using Homework.Entities.ViewModel.Category;
using Homework.Entities.ViewModel.OrderDetail;
using Homework.Entities.ViewModel.Supplier;

namespace Homework.Entities.ViewModel.Product
{
    public class ProductViewModel : ProductBaseModel
    {
        public int ProductId { get; set; }

        public CategoryViewModel Category { get; set; }
        public SupplierViewModel Supplier { get; set; }
        public IEnumerable<OrderDetailViewModel> OrderDetails { get; set; }
    }
}
