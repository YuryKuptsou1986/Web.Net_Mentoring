using Homework.DBContext.Repositories.Interfaces;
using Homework.Entities.Data;
using Microsoft.EntityFrameworkCore;

namespace Homework.DBContext.Repositories.Implementaion
{
    public class ProductRepository : BaseRepository<Product>, IProductRepository
    {
        private readonly NorthwindContext _context;

        public ProductRepository(NorthwindContext context) : base(context)
        {
            _context = context;
        }

        public override async Task<Product> GetAsync(int id)
        {
            return await _context.Set<Product>().Include(x => x.Category).Include(x => x.Supplier).FirstOrDefaultAsync(x => x.ProductId == id);
        }

        public override async Task<IEnumerable<Product>> GetTopAsync(int top)
        {
            return await _context.Set<Product>().Include(x => x.Category).Include(x => x.Supplier).Take(top).ToListAsync();
        }

        public override async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _context.Set<Product>().Include(x => x.Category).Include(x => x.Supplier).ToListAsync();
        }
    }
}
