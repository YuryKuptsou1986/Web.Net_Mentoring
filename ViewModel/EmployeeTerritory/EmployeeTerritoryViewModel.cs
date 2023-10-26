using ViewModel.Employee;
using ViewModel.Territory;

namespace ViewModel.EmployeeTerritory
{
    public class EmployeeTerritoryViewModel : EmployeeTerritoryBaseModel
    {
        public EmployeeViewModel Employee { get; set; }
        public TerritoryViewModel Territory { get; set; }
    }
}
