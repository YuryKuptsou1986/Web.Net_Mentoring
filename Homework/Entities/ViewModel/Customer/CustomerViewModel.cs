using Homework.Entities.ViewModel.Order;

namespace Homework.Entities.ViewModel.Customer
{
    public class CustomerViewModel
    {
        public string CustomerId { get; set; }
        public IEnumerable<OrderViewModel> Orders { get; set; }
    }
}
