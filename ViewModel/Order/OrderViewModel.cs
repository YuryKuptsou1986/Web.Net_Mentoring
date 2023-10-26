using ViewModel.Customer;
using ViewModel.Employee;
using ViewModel.OrderDetail;
using ViewModel.Shipper;

namespace ViewModel.Order
{
    public class OrderViewModel : OrderBaseModel
    {
        public int OrderId { get; set; }
        public CustomerViewModel Customer { get; set; }
        public EmployeeViewModel Employee { get; set; }
        public ShipperViewModel ShipViaNavigation { get; set; }
        public IEnumerable<OrderDetailViewModel> OrderDetails { get; set; }
    }
}
