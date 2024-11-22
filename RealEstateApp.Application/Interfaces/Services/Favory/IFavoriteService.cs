using RealEstateApp.Application.Interfaces.Services.Generic;
using RealEstateApp.Application.ViewModels.Favory;
using RealEstateApp.Domain.Entities;

namespace RealEstateApp.Application.Interfaces.Services.Favory;

public interface IFavoriteService : IService<Favorite>
{
    Task<bool> ToggleFavoriteAsync(int propertyId, string userId);
    Task<List<int>> GetFavoritePropertyIdsAsync(string userId);
    Task<List<FavoriteViewModel>> GetFavoritePropertiesAsync(string userId);
}