using RealEstateApp.Application.Interfaces.Repositories.Generic;
using RealEstateApp.Domain.Entities;

namespace RealEstateApp.Application.Interfaces.Repositories.PropertiesType;

public interface IPropertyTypeRepository : IRepository<PropertyType>
{
    Task<int> GetPropertiesCountAsync(int propertyTypeId);
}