using AutoMapper;
using RealEstateApp.Application.Interfaces.Repositories.Generic;
using RealEstateApp.Application.Interfaces.Repositories.SalesType;
using RealEstateApp.Application.Interfaces.Services.SalesType;
using RealEstateApp.Application.Services.Generic;
using RealEstateApp.Domain.Entities;

namespace RealEstateApp.Application.Services.SalesTypes
{
    public class SalesTypesService : Service<SaleType>, ISalesTypesService
    {
        private readonly ISaleTypeRepository _repository;
        private readonly IMapper _mapper;

        public SalesTypesService(IRepository<SaleType> repository, ISaleTypeRepository saleTypeRepository, IMapper mapper) : base(repository)
        {
            _mapper = mapper;
            _repository = saleTypeRepository;
        }

        public async Task<int> GetPropertiesCountAsync(int saleTypeId)
        {
            return await _repository.GetPropertiesCountAsync(saleTypeId);
        }
    }
}
