using System.Data;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.DependencyInjection;
using RealEstateApp.Application.Exceptions;
using RealEstateApp.Application.Helpers;
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

    public async Task<List<PropertyListViewModel>> GetAllPropertiesByUserAsync(string? userId)
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
        try
        {
            if (model == null)
                throw new ValidationException("El modelo proporcionado es nulo.");

            if (model.ImprovementIds == null || !model.ImprovementIds.Any())
                throw new ValidationException("Debe proporcionar al menos una mejora asociada.");

            if (model.AgentId == null)
                throw new ValidationException("El ID del agente es obligatorio.");
            
            
            ICollection<Improvement> improvements = new List<Improvement>();
            foreach (var id in model.ImprovementIds)
            {
                var improvement = await _improvementRepository.GetByIdAsync(id);
                if (improvement != null)
                {
                    improvements.Add(improvement);
                }
                else
                {
                    throw new ValidationException($"La mejora con ID {id} no existe.");
                }
            }

            var property = new Property
            {
                PropertyTypeId = model.PropertyTypeId,
                AgentId = model.AgentId!,
                Code = Guid.NewGuid().ToString("N").Substring(0, 6).ToUpper(),
                SaleTypeId = model.SaleTypeId,
                Price = model.Price,
                Size = model.Size,
                Rooms = model.Rooms,
                Bathrooms = model.Bathrooms,
                Description = model.Description,
                Improvements = improvements,
                Images = await FileHelper.SaveImagesAsync(model.Images, "images/properties"),
            };

            await AddAsync(property);
        }
        catch (ValidationException)
        {
            throw;
        }
        catch (DuplicateException)
        {
            throw;
        }
        catch (Exception e)
        {
            throw new DatabaseOperationException($"Se produjo un error al agregar una nueva propiedad: {e.Message}");
        }
    }
    
    
    public Task<bool> UpdatePropertyAsync(int id, PropertyUpdateViewModel updateDto)
    {
        return null;
    }



    public async Task<int> DeletePropertyAsync(int id)
    {
        return await DeleteAsync(id);
    }
}