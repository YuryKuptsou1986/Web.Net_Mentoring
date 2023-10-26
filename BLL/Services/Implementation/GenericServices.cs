using AutoMapper;
using BLL.Services.Interfaces;
using DAL.Repositories.Interfaces;

namespace BLL.Services.Implementation
{
    internal class GenericService<T, TViewModel, TCreateModel, TUpdateModel> : IGenericService<TViewModel, TCreateModel, TUpdateModel>
        where T : class
        where TViewModel : class
        where TCreateModel : class
        where TUpdateModel : class
    {
        private readonly IBaseRepository<T> _repository;
        private readonly IMapper _mapper;

        public GenericService(IBaseRepository<T> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task AddAsync(TCreateModel entity)
        {
            var createEntity = _mapper.Map<T>(entity);
            await _repository.AddAsync(createEntity);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }

        public async Task DeleteAsync(string id)
        {
            await _repository.DeleteAsync(id);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _repository.DeleteAsync(id);
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _repository.ExistsAsync(id);
        }

        public async Task<bool> ExistsAsync(string id)
        {
            return await _repository.ExistsAsync(id);
        }

        public async Task<IEnumerable<TViewModel>> GetAllAsync()
        {
            var list = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<TViewModel>>(list);
        }

        public async Task<TViewModel> GetAsync(int id)
        {
            var entity = await _repository.GetAsync(id);
            return _mapper.Map<TViewModel>(entity);
        }

        public async Task<TViewModel> GetAsync(string id)
        {
            var entity = await _repository.GetAsync(id);
            return _mapper.Map<TViewModel>(entity);
        }

        public async Task<TViewModel> GetAsync(Guid id)
        {
            var entity = await _repository.GetAsync(id);
            return _mapper.Map<TViewModel>(entity);
        }

        public async Task<IEnumerable<TViewModel>> GetTopAsync(int top)
        {
            var list = await _repository.GetTopAsync(top);
            return _mapper.Map<List<TViewModel>>(list);
        }

        public async Task UpdateAsync(TUpdateModel entity)
        {
            var updateEntity = _mapper.Map<T>(entity);
            await _repository.UpdateAsync(updateEntity);
        }
    }
}
