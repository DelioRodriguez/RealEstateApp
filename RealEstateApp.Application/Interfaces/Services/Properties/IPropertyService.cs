using RealEstateApp.Application.Interfaces.Services.Generic;
using RealEstateApp.Application.ViewModels.Agents;
using RealEstateApp.Application.ViewModels.Properties;
using RealEstateApp.Domain.Entities;

namespace RealEstateApp.Application.Interfaces.Services.Properties;

public interface IPropertyService : IService<Property>
{
    Task<List<PropertyListViewModel>> GetAvailablePropertiesAsync();
    Task<PropertyDetailViewModel> GetPropertyDetailsAsync(int id);
    Task<List<PropertyListViewModel>> SearchPropertiesAsync(PropertyFilterViewModel? filter);
    Task<PropertyByAgentViewModel> GetPropertyByUserIdAsync(string userId);
}