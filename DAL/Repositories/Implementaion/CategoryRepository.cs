using DAL.DBContext;
using DAL.Repositories.Interfaces;
using Domain.Entities;

namespace DAL.Repositories.Implementaion
{
    internal class CategoryRepository : BaseRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(INorthwindContext context) : base(context) { }
    }
}
