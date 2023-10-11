using Homework.Entities.ViewModel.Product;
using Homework.Entities.ViewModel.Order;

namespace Homework.Entities.ViewModel.OrderDetail
{
    public class OrderDetailViewModel : OrderDetailBaseModel
    {
        public int OrderId { get; set; }
        public OrderViewModel Order { get; set; }
        public ProductViewModel Product { get; set; }
    }
}
