namespace DAL.Repositories.Interfaces
{
    public interface IBaseRepository<TEntity> where TEntity : class
    {
        Task AddAsync(TEntity entity);
        Task<TEntity> GetAsync(string id);
        Task<TEntity> GetAsync(int id);
        Task<TEntity> GetAsync(Guid id);
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<IEnumerable<TEntity>> GetTopAsync(int top);
        Task UpdateAsync(TEntity entity);
        Task DeleteAsync(string id);
        Task DeleteAsync(int id);
        Task DeleteAsync(Guid id);
        Task<bool> ExistsAsync(string id);
        Task<bool> ExistsAsync(Guid id);
        Task<bool> ExistsAsync(int id);
    }
}
