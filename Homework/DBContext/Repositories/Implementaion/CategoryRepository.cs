using Homework.DBContext.Repositories.Interfaces;
using Homework.Entities.Data;

namespace Homework.DBContext.Repositories.Implementaion
{
    public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(NorthwindContext context) : base(context) { }
    }
}
