using Homework.DBContext.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Homework.DBContext.Repositories.Implementaion
{
    public abstract class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class, new()
    {
        protected readonly NorthwindContext _context;

        protected BaseRepository(NorthwindContext context)
        {
            _context = context;
        }
        public virtual async Task<TEntity> GetAsync(string id)
        {
            return await _context.Set<TEntity>().FindAsync(id);
        }
        public virtual async Task<TEntity> GetAsync(int id)
        {
            return await _context.Set<TEntity>().FindAsync(id);
        }
        public virtual async Task<TEntity> GetAsync(Guid id)
        {
            return await _context.Set<TEntity>().FindAsync(id);
        }

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _context.Set<TEntity>().ToListAsync();
        }

        public virtual async Task<IEnumerable<TEntity>> GetTopAsync(int top)
        {
            return await _context.Set<TEntity>().Take(top).ToListAsync();
        }

        public virtual async Task AddAsync(TEntity entity)
        {
            _context.Set<TEntity>().Add(entity);
            await SaveChanges();
        }

        public virtual async Task UpdateAsync(TEntity entity)
        {
            _context.Set<TEntity>().Update(entity);
            await SaveChanges();
        }

        public virtual async Task DeleteAsync(string id)
        {
            var entity = await _context.Set<TEntity>().FindAsync(id);
            _context.Set<TEntity>().Remove(entity);
            await SaveChanges();
        }

        public virtual async Task DeleteAsync(int id)
        {
            _context.Set<TEntity>().Remove(await _context.Set<TEntity>().FindAsync(id));
            await SaveChanges();
        }
        public virtual async Task DeleteAsync(Guid id)
        {
            _context.Set<TEntity>().Remove(await _context.Set<TEntity>().FindAsync(id));
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
