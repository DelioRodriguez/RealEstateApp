using RealEstateApp.Application.Interfaces.Services.Generic;
using RealEstateApp.Application.ViewModels.Improvements;
using RealEstateApp.Domain.Entities;

namespace RealEstateApp.Application.Interfaces.Services.Improvements;

public interface IImprovementService : IService<Improvement>
{
    public Task<List<ImprovementViewModel>> GetImprovementsByPropertyIdAsync(int propertyId);
}