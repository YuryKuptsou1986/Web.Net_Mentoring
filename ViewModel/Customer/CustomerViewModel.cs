using ViewModel.Order;

namespace ViewModel.Customer
{
    public class CustomerViewModel
    {
        public string CustomerId { get; set; }
        public IEnumerable<OrderViewModel> Orders { get; set; }
    }
}
