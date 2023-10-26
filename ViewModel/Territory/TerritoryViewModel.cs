using ViewModel.EmployeeTerritory;
using ViewModel.Region;

namespace ViewModel.Territory
{
    public class TerritoryViewModel : TerritoryBaseModel
    {
        public string TerritoryId { get; set; }
        public RegionViewMode Region { get; set; }
        public IEnumerable<EmployeeTerritoryViewModel> EmployeeTerritories { get; set; }
    }
}
