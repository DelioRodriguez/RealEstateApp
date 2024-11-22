using AutoMapper;
using RealEstateApp.Application.Interfaces.Repositories.Generic;
using RealEstateApp.Application.Interfaces.Repositories.Properties;
using RealEstateApp.Application.Interfaces.Repositories.Users;
using RealEstateApp.Application.Interfaces.Services.Properties;
using RealEstateApp.Application.Services.Generic;
using RealEstateApp.Application.ViewModels.Agents;
using RealEstateApp.Application.ViewModels.Properties;
using RealEstateApp.Application.ViewModels.Users;
using RealEstateApp.Domain.Entities;

namespace RealEstateApp.Application.Services.Properties;

public class PropertyService : Service<Property>, IPropertyService
{
    private readonly IPropertyRepository _propertyRepository;
    private readonly IMapper _mapper;
    private readonly IUserRepository _repository;
    
    public PropertyService(IRepository<Property> repository, IPropertyRepository propertyRepository, IMapper mapper, IUserRepository repository1) : base(repository)
    {
        _propertyRepository = propertyRepository;
        _mapper = mapper;
        _repository = repository1;
    }
    
    
    public async Task<List<PropertyListViewModel>> GetAvailablePropertiesAsync()
    {
        var properties = await _propertyRepository.GetAvailablePropertiesAsync();
        
        return _mapper.Map<List<PropertyListViewModel>>(properties);
    }

    public async Task<PropertyDetailViewModel> GetPropertyDetailsAsync(int id)
    {
        var property = await _propertyRepository.GetPropertyDetailsAsync(id);
        var mapperProperty = _mapper.Map<PropertyDetailViewModel>(property);
        var user = await _repository.GetUserByIdAsync(property!.AgentId);

        mapperProperty.AgentEmail = user.AgentEmail;
        mapperProperty.AgentName = user.AgentName;
        mapperProperty.AgentPhone = user.AgentPhone;
        mapperProperty.AgentImageUrl = user.AgentImageUrl;
        mapperProperty.AgentId = user.AgentId;

        return mapperProperty;
    }

    public async Task<List<PropertyListViewModel>> SearchPropertiesAsync(PropertyFilterViewModel? filter)
    {
        var properties = await _propertyRepository.SearchPropertiesAsync(filter);
        
        
        return _mapper.Map<List<PropertyListViewModel>>(properties);
    }

    public async Task<PropertyByAgentViewModel> GetPropertyByUserIdAsync(string userId)
    {
        var user = await _repository.GetUserByIdAsync(userId);

        var propityByUser = new PropertyByAgentViewModel()
        {
            Agent = _mapper.Map<AgentViewModel>(user),
            Properties =
                _mapper.Map<List<PropertyListViewModel>>(await _propertyRepository.GetPropertyByUserIdAsync(userId))!
        };
        
        return propityByUser; 
    }
}