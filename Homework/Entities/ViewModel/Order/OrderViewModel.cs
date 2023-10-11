using Homework.Entities.ViewModel.Customer;
using Homework.Entities.ViewModel.Employee;
using Homework.Entities.ViewModel.OrderDetail;
using Homework.Entities.ViewModel.Shipper;

namespace Homework.Entities.ViewModel.Order
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
