using AutoMapper;
using RealEstateApp.Application.Interfaces.Repositories.Api;
using RealEstateApp.Application.Interfaces.Repositories.Generic;
using RealEstateApp.Application.Interfaces.Services.Api;
using RealEstateApp.Application.Services.Generic;
using RealEstateApp.Domain.Entities;

namespace RealEstateApp.Application.Services.Api
{
    public class SaleTypeApiService : Service<SaleType>, ISaleTypeApiService
    {
        private readonly ISaleTypeApiRepository _saleTypeRepository;
        private readonly IMapper _mapper;

        public SaleTypeApiService(IRepository<SaleType> repository, ISaleTypeApiRepository saleTypeRepository, IMapper mapper ) : base( repository ) 
        {
            _saleTypeRepository = saleTypeRepository;
            _mapper = mapper;
        }

    }
}
