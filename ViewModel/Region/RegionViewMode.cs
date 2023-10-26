using ViewModel.Territory;

namespace ViewModel.Region
{
    public class RegionViewMode : RegionBaseModel
    {
        public IEnumerable<TerritoryViewModel> Territories { get; set; }
    }
}
