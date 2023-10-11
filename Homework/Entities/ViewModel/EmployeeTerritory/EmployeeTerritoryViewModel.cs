using Homework.Entities.ViewModel.Employee;
using Homework.Entities.ViewModel.Territory;

namespace Homework.Entities.ViewModel.EmployeeTerritory
{
    public class EmployeeTerritoryViewModel : EmployeeTerritoryBaseModel
    {
        public EmployeeViewModel Employee { get; set; }
        public TerritoryViewModel Territory { get; set; }
    }
}
