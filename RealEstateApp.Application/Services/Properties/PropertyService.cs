using AutoMapper;
using RealEstateApp.Application.Interfaces.Repositories.Generic;
using RealEstateApp.Application.Interfaces.Repositories.Properties;
using RealEstateApp.Application.Interfaces.Repositories.Users;
using RealEstateApp.Application.Interfaces.Services.Properties;
using RealEstateApp.Application.Services.Generic;
using RealEstateApp.Application.ViewModels.Properties;
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
    
    
    public async Task<IEnumerable<PropertyListViewModel>> GetAvailablePropertiesAsync()
    {
        var properties = await _propertyRepository.GetAvailablePropertiesAsync();
        
        return _mapper.Map<IEnumerable<PropertyListViewModel>>(properties);
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

        return mapperProperty;
    }

    public async Task<IEnumerable<PropertyListViewModel>> SearchPropertiesAsync(PropertyFilterViewModel filter)
    {
        var properties = await _propertyRepository.SearchPropertiesAsync(filter);
        
        
        return _mapper.Map<IEnumerable<PropertyListViewModel>>(properties);
    }
}