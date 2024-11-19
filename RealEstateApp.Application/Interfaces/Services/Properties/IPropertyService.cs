using RealEstateApp.Application.Interfaces.Services.Generic;
using RealEstateApp.Application.ViewModels.Properties;
using RealEstateApp.Domain.Entities;

namespace RealEstateApp.Application.Interfaces.Services.Properties;

public interface IPropertyService : IService<Property>
{
    Task<IEnumerable<PropertyListViewModel>> GetAvailablePropertiesAsync();
    Task<PropertyDetailViewModel> GetPropertyDetailsAsync(int id);
    Task<IEnumerable<PropertyListViewModel>> SearchPropertiesAsync(PropertyFilterViewModel filter);
}