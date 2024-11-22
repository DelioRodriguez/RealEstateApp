using RealEstateApp.Application.Interfaces.Repositories.Generic;
using RealEstateApp.Domain.Entities;

namespace RealEstateApp.Application.Interfaces.Repositories.Favory;

public interface IFavoriteRepository : IRepository<Favorite>
{
    Task<Favorite?> GetFavoriteAsync(int propertyId, string userId);
    Task<IEnumerable<Favorite>> GetFavoritesByUserAsync(string userId);
    Task AddFavoriteAsync(Favorite favorite);
    Task RemoveFavoriteAsync(Favorite favorite);
    Task<bool> SaveChangesAsync();
}