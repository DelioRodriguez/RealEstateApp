using RealEstateApp.Application.Interfaces.Repositories.Favory;
using RealEstateApp.Application.Interfaces.Repositories.Generic;
using RealEstateApp.Application.Interfaces.Services.Favory;
using RealEstateApp.Application.Services.Generic;
using RealEstateApp.Application.ViewModels.Favory;
using RealEstateApp.Domain.Entities;

namespace RealEstateApp.Application.Services.Favory;

public class FavoriteService : Service<Favorite>, IFavoriteService
{
    private readonly IFavoriteRepository _favoriteRepository;
    
    public FavoriteService(IRepository<Favorite> repository, IFavoriteRepository repository1, IFavoriteRepository favoriteRepository) : base(repository)
    {
        _favoriteRepository = favoriteRepository;
    }
    
    public async Task<bool> ToggleFavoriteAsync(int propertyId, string userId)
    {
        var favorite = await _favoriteRepository.GetFavoriteAsync(propertyId, userId);

        if (favorite != null)
        {
            await _favoriteRepository.RemoveFavoriteAsync(favorite); // Desmarcar favorito
        }
        else
        {
            var newFavorite = new Favorite
            {
                PropertyId = propertyId,
                UserId = userId,
                AddedOn = DateTime.UtcNow
            };
            await _favoriteRepository.AddFavoriteAsync(newFavorite); // Marcar favorito
        }

        return await _favoriteRepository.SaveChangesAsync();
    }

    public async Task<List<int>> GetFavoritePropertyIdsAsync(string userId)
    {
        var favorites = await _favoriteRepository.GetFavoritesByUserAsync(userId);
        return favorites.Select(f => f.PropertyId).ToList();
    }

    public async Task<List<FavoriteViewModel>> GetFavoritePropertiesAsync(string userId)
    {
        var favorites = await _favoriteRepository.GetFavoritesByUserAsync(userId);

        return favorites.Select(i => new FavoriteViewModel
        {
            Id = i.Id,
            UserId = i.UserId,
            AddedOn = i.AddedOn,
            PropertyId = i.PropertyId
        }).ToList();
    }
}