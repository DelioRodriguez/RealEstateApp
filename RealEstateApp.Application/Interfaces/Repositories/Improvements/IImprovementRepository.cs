using RealEstateApp.Application.Interfaces.Repositories.Generic;
using RealEstateApp.Domain.Entities;

namespace RealEstateApp.Application.Interfaces.Repositories.Improvements;

public interface IImprovementRepository : IRepository<Improvement>
{
    public Task<List<Improvement>> GetImprovementsByPropertyIdAsync(int propertyId);
}