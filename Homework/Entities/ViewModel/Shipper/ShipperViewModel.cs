using Homework.Entities.ViewModel.Order;

namespace Homework.Entities.ViewModel.Shipper
{
    public class ShipperViewModel : ShipperBaseModel
    {
        public int ShipperId { get; set; }
        public IEnumerable<OrderViewModel> Orders { get; set; }
    }
}
