using RealEstateApp.Application.Interfaces.Services.Generic;
using RealEstateApp.Domain.Entities;
namespace RealEstateApp.Application.Interfaces.Services.PropertiesType
{
    public interface IPropertiesTypeServices : IService<PropertyType>
    {
        Task<int> GetPropertiesCountAsync(int propertyTypeId);
    }
}
