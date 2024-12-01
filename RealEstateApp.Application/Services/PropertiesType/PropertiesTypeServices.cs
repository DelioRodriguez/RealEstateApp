using AutoMapper;
using RealEstateApp.Application.Interfaces.Repositories.Generic;
using RealEstateApp.Application.Interfaces.Repositories.PropertiesType;
using RealEstateApp.Application.Interfaces.Services.PropertiesType;
using RealEstateApp.Application.Services.Generic;
using RealEstateApp.Domain.Entities;

namespace RealEstateApp.Application.Services.PropertiesType
{
    public class PropertiesTypeServices : Service<PropertyType>, IPropertiesTypeServices
    {
        private readonly IPropertyTypeRepository _repository;
        private readonly IMapper _mapper;

        public PropertiesTypeServices(IRepository<PropertyType> repository, IPropertyTypeRepository PropertyTypeRepository, IMapper mapper) : base(repository)
        {
            _repository = PropertyTypeRepository;
            _mapper = mapper;
        }

        public async Task<int> GetPropertiesCountAsync(int propertyTypeId)
        {
            return await _repository.GetPropertiesCountAsync(propertyTypeId);
        }
    }
}
