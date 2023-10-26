using DAL.DBContext;
using DAL.Repositories.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories.Implementaion
{
    internal class ProductRepository : BaseRepository<Product>, IProductRepository
    {
        private readonly INorthwindContext _context;

        public ProductRepository(INorthwindContext context) : base(context)
        {
            _context = context;
        }

        public override async Task<Product> GetAsync(int id)
        {
            return await _context.SetEntityContext<Product>().Include(x => x.Category).Include(x => x.Supplier).FirstOrDefaultAsync(x => x.ProductId == id);
        }

        public override async Task<IEnumerable<Product>> GetTopAsync(int top)
        {
            return await _context.SetEntityContext<Product>().Include(x => x.Category).Include(x => x.Supplier).Take(top).ToListAsync();
        }

        public override async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _context.SetEntityContext<Product>().Include(x => x.Category).Include(x => x.Supplier).ToListAsync();
        }
    }
}
