using Microsoft.EntityFrameworkCore;
using RealEstateApp.Application.Interfaces.Repositories.Properties;
using RealEstateApp.Application.ViewModels.Properties;
using RealEstateApp.Domain.Entities;
using RealEstateApp.Infrastructure.Persistance.Context;

namespace RealEstateApp.Infrastructure.Persistance.Repositories.Properties;

public class PropertyRepository : Repository<Property>, IPropertyRepository
{
    private readonly AppDbContext _context;

    public PropertyRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<List<Property?>> GetAvailablePropertiesAsync()
    {
        return (await _context.Properties
            .Include(p => p.PropertyType)
            .Include(p => p.SaleType)
            .Include(p => p.Images)
            .Where(p => p.IsAvailable)
            .ToListAsync())!;
    }

    public async Task<List<Property?>> GetAllPropertiesByUserAsync(string? userId)
    {
        return (await _context.Properties
            .Include(p => p.PropertyType)
            .Include(p => p.SaleType)
            .Include(p => p.Images)
            .Where(p => p.AgentId == userId)
            .ToListAsync())!;
    }

    public async Task<Property?> GetPropertyDetailsAsync(int id)
    {
        return await _context.Properties
            .Include(p => p.PropertyType)
            .Include(p => p.SaleType)
            .Include(p => p.Images)
            .Include(p => p.Improvements)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<IEnumerable<Property?>> SearchPropertiesAsync(PropertyFilterViewModel? filter)
    {
        var query = _context.Properties
            .Include(p => p.PropertyType)
            .Include(p => p.SaleType)
            .Include(p => p.Images)
            .Where(p => p.IsAvailable)
            .AsQueryable();

        if (!string.IsNullOrEmpty(filter!.Code))
        {
            query = query.Where(p => p.Code == filter.Code);
        }

        if (filter.PropertyTypeId.HasValue)
        {
            query = query.Where(p => p.PropertyTypeId == filter.PropertyTypeId.Value);
        }

        if (filter.SaleTypeId.HasValue)
        {
            query = query.Where(p => p.SaleTypeId == filter.SaleTypeId.Value);
        }

        if (filter.MinPrice.HasValue)
        {
            query = query.Where(p => p.Price >= filter.MinPrice.Value);
        }

        if (filter.MaxPrice.HasValue)
        {
            query = query.Where(p => p.Price <= filter.MaxPrice.Value);
        }

        if (filter.MinRooms.HasValue)
        {
            query = query.Where(p => p.Rooms >= filter.MinRooms.Value);
        }

        if (filter.MinBathrooms.HasValue)
        {
            query = query.Where(p => p.Bathrooms >= filter.MinBathrooms.Value);
        }

        if (filter.IsAvailable.HasValue)
        {
            query = query.Where(p => p.IsAvailable == filter.IsAvailable.Value);
        }

        return await query.ToListAsync();
    }

    public async Task<IEnumerable<Property?>> GetPropertyByUserIdAsync(string userId)
    {
        return await _context.Properties
            .Where(i => i.AgentId == userId && i.IsAvailable)
            .Include(p => p.PropertyType)
            .Include(p => p.SaleType)
            .Include(p => p.Images)
            .Include(p => p.Improvements)
            .ToListAsync();
    }
}