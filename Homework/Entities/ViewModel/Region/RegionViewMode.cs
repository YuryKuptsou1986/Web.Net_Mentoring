using Homework.Entities.ViewModel.Territory;

namespace Homework.Entities.ViewModel.Region
{
    public class RegionViewMode : RegionBaseModel
    {
        public IEnumerable<TerritoryViewModel> Territories { get; set; }
    }
}
