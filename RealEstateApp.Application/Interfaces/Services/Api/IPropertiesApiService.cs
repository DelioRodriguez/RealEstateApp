using RealEstateApp.Application.Dtos.Properties;
using RealEstateApp.Application.Interfaces.Services.Generic;
using RealEstateApp.Domain.Entities;

namespace RealEstateApp.Application.Interfaces.Services.Api
{
    public interface IPropertiesApiService : IService<Property>
    {
        Task<List<PropertyDto>> GetAllPropertiesAsync();
        Task<PropertyDetailsDto> GetByCodeAsync(string code);
        Task<PropertyDetailsDto> GetPropertyDetailsAsync(int propertyId);
    }
}
