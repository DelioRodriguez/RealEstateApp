using AutoMapper;
using RealEstateApp.Application.Dtos.Properties;
using RealEstateApp.Application.Interfaces.Repositories.Api;
using RealEstateApp.Application.Interfaces.Repositories.Generic;
using RealEstateApp.Application.Interfaces.Repositories.Users;
using RealEstateApp.Application.Interfaces.Services.Api;
using RealEstateApp.Application.Services.Generic;
using RealEstateApp.Domain.Entities;

namespace RealEstateApp.Application.Services.Api
{
    public class PropertiesApiService : Service<Property>, IPropertiesApiService
    {
        private readonly IPropertiesApiRepository _propertyApiRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public PropertiesApiService(IRepository<Property> repository, IPropertiesApiRepository propertiesApiRepository, IUserRepository userRepository, IMapper mapper) : base(repository)
        {
            _propertyApiRepository = propertiesApiRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<List<PropertyDto>> GetAllPropertiesAsync()
        {
            var properties = await _propertyApiRepository.GetAllPropertiesAsync();
            var mappedProperties = properties.Select(property =>
            {
                var mapped = _mapper.Map<PropertyDto>(property);
                var agent = _userRepository.GetUserByIdAsync(property.AgentId).Result;
                mapped.AgentId = agent?.AgentId;
                mapped.AgentName = agent?.AgentName;

                return mapped;
            }).ToList();

            return mappedProperties;
        }

        public async Task<PropertyDetailsDto> GetPropertyDetailsAsync(int id)
        {
            var property = await _propertyApiRepository.GetByIdWithImprovementsAsync(id);
            var map = _mapper.Map<PropertyDetailsDto>(property);
            var agent = await _userRepository.GetUserByIdAsync(property.AgentId);

            map.AgentId = agent.AgentId;
            map.AgentName = agent.AgentName;

            return map;
        }

        public async Task<PropertyDetailsDto> GetByCodeAsync(string code)
        {
            var property =  await _propertyApiRepository.GetByCodeAsync(code);
            var map = _mapper.Map<PropertyDetailsDto>(property);
            var agent = await _userRepository.GetUserByIdAsync(property.AgentId);

            map.AgentId = agent.AgentId;
            map.AgentName = agent.AgentName;

            return map;
        } 
    }
}
