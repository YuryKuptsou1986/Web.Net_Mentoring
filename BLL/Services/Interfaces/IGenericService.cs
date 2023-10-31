namespace BLL.Services.Interfaces
{
    public interface IGenericService<TViewModel, TCreateModel, TUpdateModel>
        where TViewModel : class
        where TCreateModel : class
        where TUpdateModel : class
    {
        Task<TViewModel> GetAsync(int id);
        Task<TViewModel> GetAsync(string id);
        Task<TViewModel> GetAsync(Guid id);
        Task<IEnumerable<TViewModel>> GetTopAsync(int top);
        Task<IEnumerable<TViewModel>> GetAllAsync();
        Task<int> AddAsync(TCreateModel entity);
        Task DeleteAsync(int id);
        Task DeleteAsync(string id);
        Task DeleteAsync(Guid id);
        Task UpdateAsync(TUpdateModel entity);
        Task<bool> ExistsAsync(int id);
    }
}
