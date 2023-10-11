using Homework.Entities.ViewModel.EmployeeTerritory;
using Homework.Entities.ViewModel.Region;

namespace Homework.Entities.ViewModel.Territory
{
    public class TerritoryViewModel : TerritoryBaseModel
    {
        public string TerritoryId { get; set; }
        public RegionViewMode Region { get; set; }
        public IEnumerable<EmployeeTerritoryViewModel> EmployeeTerritories { get; set; }
    }
}
