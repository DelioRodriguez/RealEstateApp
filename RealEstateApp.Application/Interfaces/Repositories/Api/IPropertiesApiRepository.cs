using RealEstateApp.Application.Interfaces.Repositories.Generic;
using RealEstateApp.Domain.Entities;

namespace RealEstateApp.Application.Interfaces.Repositories.Api
{
    public interface IPropertiesApiRepository : IRepository<Property>
    {
        Task<List<Property>> GetAllPropertiesAsync();
        Task<Property> GetByIdWithImprovementsAsync(int Id);
        Task<Property?> GetByCodeAsync(string code);
    }
}
