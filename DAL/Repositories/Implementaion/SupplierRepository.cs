using DAL.DBContext;
using DAL.Repositories.Interfaces;
using Domain.Entities;

namespace DAL.Repositories.Implementaion
{
    internal class SupplierRepository : BaseRepository<Supplier>, ISupplierRepository
    {
        public SupplierRepository(INorthwindContext context) : base(context) { }
    }
}
