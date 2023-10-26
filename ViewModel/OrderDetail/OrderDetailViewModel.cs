using ViewModel.Product;
using ViewModel.Order;

namespace ViewModel.OrderDetail
{
    public class OrderDetailViewModel : OrderDetailBaseModel
    {
        public int OrderId { get; set; }
        public OrderViewModel Order { get; set; }
        public ProductViewModel Product { get; set; }
    }
}
