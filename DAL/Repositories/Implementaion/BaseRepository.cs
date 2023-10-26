using DAL.DBContext;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories.Implementaion
{
    internal abstract class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class, new()
    {
        protected readonly INorthwindContext _context;

        protected BaseRepository(INorthwindContext context)
        {
            _context = context;
        }
        public virtual async Task<TEntity> GetAsync(string id)
        {
            return await _context.SetEntityContext<TEntity>().FindAsync(id);
        }
        public virtual async Task<TEntity> GetAsync(int id)
        {
            return await _context.SetEntityContext<TEntity>().FindAsync(id);
        }
        public virtual async Task<TEntity> GetAsync(Guid id)
        {
            return await _context.SetEntityContext<TEntity>().FindAsync(id);
        }

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _context.SetEntityContext<TEntity>().ToListAsync();
        }

        public virtual async Task<IEnumerable<TEntity>> GetTopAsync(int top)
        {
            return await _context.SetEntityContext<TEntity>().Take(top).ToListAsync();
        }

        public virtual async Task AddAsync(TEntity entity)
        {
            _context.SetEntityContext<TEntity>().Add(entity);
            await SaveChanges();
        }

        public virtual async Task UpdateAsync(TEntity entity)
        {
            _context.SetEntityContext<TEntity>().Update(entity);
            await SaveChanges();
        }

        public virtual async Task DeleteAsync(string id)
        {
            var entity = await _context.SetEntityContext<TEntity>().FindAsync(id);
            _context.SetEntityContext<TEntity>().Remove(entity);
            await SaveChanges();
        }

        public virtual async Task DeleteAsync(int id)
        {
            _context.SetEntityContext<TEntity>().Remove(await _context.SetEntityContext<TEntity>().FindAsync(id));
            await SaveChanges();
        }
        public virtual async Task DeleteAsync(Guid id)
        {
            _context.SetEntityContext<TEntity>().Remove(await _context.SetEntityContext<TEntity>().FindAsync(id));
            await SaveChanges();
        }

        public virtual async Task<bool> ExistsAsync(string id)
        {
            var result = await GetAsync(id);
            return result is null ? false : true;
        }

        public virtual async Task<bool> ExistsAsync(Guid id)
        {
            var result = await GetAsync(id);
            return result is null ? false : true;
        }

        public virtual async Task<bool> ExistsAsync(int id)
        {
            var result = await GetAsync(id);
            return result is null ? false : true;
        }

        private async Task<int> SaveChanges()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
