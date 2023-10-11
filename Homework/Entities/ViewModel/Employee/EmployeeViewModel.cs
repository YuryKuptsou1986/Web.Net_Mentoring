using Homework.Entities.ViewModel.EmployeeTerritory;
using Homework.Entities.ViewModel.Order;

namespace Homework.Entities.ViewModel.Employee
{
    public class EmployeeViewModel : EmployeeBaseModel
    {
        public int EmployeeId { get; set; }
        public EmployeeViewModel Manager { get; set; }
        public IEnumerable<EmployeeTerritoryViewModel> EmployeeTerritories { get; set; }
        public IEnumerable<EmployeeViewModel> DirectReports { get; set; }
        public IEnumerable<OrderViewModel> Orders { get; set; }
    }
}
