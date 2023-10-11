using Homework.DBContext.Repositories.Interfaces;
using Homework.Entities.Data;

namespace Homework.DBContext.Repositories.Implementaion
{
    public class SupplierRepository : BaseRepository<Supplier>, ISupplierRepository
    {
        public SupplierRepository(NorthwindContext context) : base(context) { }
    }
}
