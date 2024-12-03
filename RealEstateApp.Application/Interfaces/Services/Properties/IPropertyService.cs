using RealEstateApp.Application.Interfaces.Services.Generic;
using RealEstateApp.Application.ViewModels.Agents;
using RealEstateApp.Application.ViewModels.Properties;
using RealEstateApp.Domain.Entities;

namespace RealEstateApp.Application.Interfaces.Services.Properties;

public interface IPropertyService : IService<Property>
{
    Task<List<PropertyListViewModel>> GetAllPropertyByUserIdAsync(string userId);
    Task<PropertyCreateViewModel> GetCreatePropertyViewModelAsync();
    Task<List<PropertyListViewModel>> GetAvailablePropertiesAsync();
    Task<List<PropertyListViewModel>> GetAllPropertiesByUserAsync(string? userId);
    Task<PropertyDetailViewModel> GetPropertyDetailsAsync(int id);
    Task<List<PropertyListViewModel>> SearchPropertiesAsync(PropertyFilterViewModel? filter);
    Task<PropertyByAgentViewModel> GetPropertyByUserIdAsync(string userId);
    Task AddPropertyAsync(PropertyCreateViewModel property);
    Task<int> DeletePropertyAsync(int id);
}