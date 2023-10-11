using AutoMapper;
using Homework.DBContext.Repositories.Interfaces;
using Homework.Entities.Data;
using Homework.Entities.ViewModel.Supplier;
using Homework.Services.Interfaces;

namespace Homework.Services.Implementation
{
    public class SupplierService : GenericService<Supplier,
                                        SupplierViewModel,
                                        SupplierCreateModel,
                                        SupplierUpdateModel>, ISupplierService
    {
        public SupplierService(ISupplierRepository repository, IMapper mapper) : base(repository, mapper) { }
    }
}
