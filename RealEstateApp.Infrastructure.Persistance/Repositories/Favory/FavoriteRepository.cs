using Microsoft.EntityFrameworkCore;
using RealEstateApp.Application.Interfaces.Repositories.Favory;
using RealEstateApp.Domain.Entities;
using RealEstateApp.Infrastructure.Persistance.Context;

namespace RealEstateApp.Infrastructure.Persistance.Repositories.Favory;

public class FavoriteRepository : Repository<Favorite>, IFavoriteRepository
{
    private readonly AppDbContext _context;
    public FavoriteRepository(AppDbContext context) : base(context)
    {
        this._context = context;
    }
   
    
    public async Task<Favorite?> GetFavoriteAsync(int propertyId, string userId)
    {
        return await _context.Favorites
            .FirstOrDefaultAsync(f => f.PropertyId == propertyId && f.UserId == userId);
    }

    public async Task<IEnumerable<Favorite>> GetFavoritesByUserAsync(string userId)
    {
        return await _context.Favorites
            .Where(f => f.UserId == userId)
            .ToListAsync();
    }

    public async Task AddFavoriteAsync(Favorite favorite)
    {
        await _context.Favorites.AddAsync(favorite);
    }

    public Task RemoveFavoriteAsync(Favorite favorite)
    {
        _context.Favorites.Remove(favorite);
        return Task.CompletedTask;
    }

    public async Task<bool> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync() > 0;
    }
}