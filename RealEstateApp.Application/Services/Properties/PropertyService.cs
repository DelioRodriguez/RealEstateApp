using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using RealEstateApp.Application.Interfaces.Repositories.Generic;
using RealEstateApp.Application.Interfaces.Repositories.Improvements;
using RealEstateApp.Application.Interfaces.Repositories.Properties;
using RealEstateApp.Application.Interfaces.Repositories.PropertiesType;
using RealEstateApp.Application.Interfaces.Repositories.SalesType;
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
    private readonly IPropertyTypeRepository _propertyTypeRepository;
    private readonly ISaleTypeRepository _saleTypeRepository;
    private readonly IImprovementRepository _improvementRepository;
    
    public PropertyService(IRepository<Property> repository, IPropertyRepository propertyRepository, IMapper mapper, IUserRepository repository1, IPropertyTypeRepository propertyTypeRepository, IImprovementRepository improvementRepository, ISaleTypeRepository saleTypeRepository) : base(repository)
    {
        _propertyRepository = propertyRepository;
        _mapper = mapper;
        _repository = repository1;
        _propertyTypeRepository = propertyTypeRepository;
        _improvementRepository = improvementRepository;
        _saleTypeRepository = saleTypeRepository;
    }
    
    
    public async Task<List<PropertyListViewModel>> GetAvailablePropertiesAsync()
    {
        var properties = await _propertyRepository.GetAvailablePropertiesAsync();
        
        return _mapper.Map<List<PropertyListViewModel>>(properties);
    }

    public async Task<List<PropertyListViewModel>> GetAllPropertiesByUserAsync(string userId)
    {
        return _mapper.Map<List<PropertyListViewModel>>(await _propertyRepository.GetAllPropertiesByUserAsync(userId));
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
    
    public async Task<PropertyCreateViewModel> GetCreatePropertyViewModelAsync()
    {
        var propertyTypes = await _propertyTypeRepository.GetAllAsync();
        var saleTypes = await _saleTypeRepository.GetAllAsync();
        var improvements = await _improvementRepository.GetAllAsync();

        return new PropertyCreateViewModel
        {
            PropertyTypes = propertyTypes.Select(pt => new SelectListItem
            {
                Value = pt.Id.ToString(),
                Text = pt.Name
            }),
            SaleTypes = saleTypes.Select(st => new SelectListItem
            {
                Value = st.Id.ToString(),
                Text = st.Name
            }),
            Improvements = improvements.Select(i => new SelectListItem
            {
                Value = i.Id.ToString(),
                Text = i.Name
            })
        };
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

    public async Task AddPropertyAsync(PropertyCreateViewModel model)
    {
        var property = new Property
        {
            PropertyTypeId = model.PropertyTypeId,
            SaleTypeId = model.SaleTypeId,
            Price = model.Price,
            Size = model.Size,
            Rooms = model.Rooms,
            Bathrooms = model.Bathrooms,
            Description = model.Description,
            Improvements = model.ImprovementIds.Select(id => new Improvement { Id = id }).ToList(),
            Images = await SaveImagesAsync(model.Images)
        };

        await AddAsync(property);
    }

    private async Task<List<PropertyImage>> SaveImagesAsync(IEnumerable<IFormFile> images)
    {
        var propertyImages = new List<PropertyImage>();
        var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/properties");
        
        if (!Directory.Exists(uploadPath))
        {
            Directory.CreateDirectory(uploadPath);
        }

        foreach (var image in images)
        {
            var fileName = $"{Guid.NewGuid()}_{image.FileName}";
            var filePath = Path.Combine(uploadPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(stream);
            }
            
            propertyImages.Add(new PropertyImage
            {
                ImageUrl = $"/images/properties/{fileName}"
            });
        }

        return propertyImages;
    }

}