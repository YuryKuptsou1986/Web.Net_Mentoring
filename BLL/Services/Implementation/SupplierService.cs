using AutoMapper;
using BLL.Services.Interfaces;
using DAL.Repositories.Interfaces;
using Domain.Entities;
using ViewModel.Supplier;

namespace BLL.Services.Implementation
{
    internal class SupplierService : GenericService<Supplier,
                                        SupplierViewModel,
                                        SupplierCreateModel,
                                        SupplierUpdateModel>, ISupplierService
    {
        public SupplierService(ISupplierRepository repository, IMapper mapper) : base(repository, mapper) { }

        public override async Task<int> AddAsync(SupplierCreateModel entity)
        {
            var createEntity = _mapper.Map<Supplier>(entity);
            await _repository.AddAsync(createEntity);
            return createEntity.SupplierId;
        }
    }
}
