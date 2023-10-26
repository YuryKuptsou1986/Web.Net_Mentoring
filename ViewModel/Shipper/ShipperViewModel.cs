using ViewModel.Order;

namespace ViewModel.Shipper
{
    public class ShipperViewModel : ShipperBaseModel
    {
        public int ShipperId { get; set; }
        public IEnumerable<OrderViewModel> Orders { get; set; }
    }
}
