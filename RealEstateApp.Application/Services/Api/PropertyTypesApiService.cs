using AutoMapper;
using RealEstateApp.Application.Interfaces.Repositories.Api;
using RealEstateApp.Application.Interfaces.Repositories.Generic;
using RealEstateApp.Application.Interfaces.Services.Api;
using RealEstateApp.Application.Services.Generic;
using RealEstateApp.Domain.Entities;

namespace RealEstateApp.Application.Services.Api
{
    public class PropertyTypesApiService : Service<PropertyType>, IPropertyTypesApiService
    {
        private readonly IPropertyTypesApiRepository _propertyTypesRepository;
        private readonly IMapper _mapper;

        public PropertyTypesApiService(IRepository<PropertyType> repository, IPropertyTypesApiRepository propertyTypesApiRepository, IMapper mapper) : base(repository) 
        { 
            _propertyTypesRepository = propertyTypesApiRepository;
            _mapper = mapper;
        }
    }
}
