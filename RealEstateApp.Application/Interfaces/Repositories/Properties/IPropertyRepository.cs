using RealEstateApp.Application.Interfaces.Repositories.Generic;
using RealEstateApp.Application.ViewModels.Properties;
using RealEstateApp.Domain.Entities;

namespace RealEstateApp.Application.Interfaces.Repositories.Properties;

public interface IPropertyRepository : IRepository<Property>
{
    Task<List<Property?>> GetAvailablePropertiesAsync();
    Task<List<Property?>> GetAllPropertiesByUserAsync(string? userId);
    Task<Property?> GetPropertyDetailsAsync(int id);
    Task<IEnumerable<Property?>> SearchPropertiesAsync(PropertyFilterViewModel? filter);
    Task<IEnumerable<Property?>> GetPropertyByUserIdAsync(string userId);
    Task<List<Property?>> GetAllPropertyByUserAsync(string? userId);        

    Task SaveChangesAsync();

}