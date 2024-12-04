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
    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
    public async Task<List<Property?>> GetAllPropertyByUserAsync(string? userId)
    {
        return (await _context.Properties
            .Include(p => p.PropertyType)
            .Include(p => p.SaleType)
            .Include(p => p.Images)
            .Where(p => p.AgentId == userId  && p.IsAvailable)
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

    public async Task UpdatePropertyAsync(Property property)
    {
        var existingProperty = await _context.Properties
            .Include(p => p.Improvements)
            .Include(p => p.Images)
            .FirstOrDefaultAsync(p => p.Id == property.Id);

        if (existingProperty == null)
        {
            throw new KeyNotFoundException($"Property with ID {property.Id} not found.");
        }

        existingProperty.PropertyTypeId = property.PropertyTypeId;
        existingProperty.SaleTypeId = property.SaleTypeId;
        existingProperty.Price = property.Price;
        existingProperty.Size = property.Size;
        existingProperty.Rooms = property.Rooms;
        existingProperty.Bathrooms = property.Bathrooms;
        existingProperty.Description = property.Description;
        existingProperty.IsAvailable = property.IsAvailable;

        if (property.Improvements != null)
        {
            var currentImprovementIds = existingProperty.Improvements.Select(i => i.Id).ToList();

            var improvementsToAdd = property.Improvements
                .Where(i => !currentImprovementIds.Contains(i.Id))
                .ToList();

            var improvementsToRemove = existingProperty.Improvements
                .Where(i => !property.Improvements.Any(pi => pi.Id == i.Id))
                .ToList();

            foreach (var improvement in improvementsToRemove)
            {
                existingProperty.Improvements.Remove(improvement);
            }

            foreach (var improvement in improvementsToAdd)
            {
                existingProperty.Improvements.Add(improvement);
            }
        }
        else
        {
            existingProperty.Improvements.Clear();
        }

        if (property.Images != null)
        {
            var imagesToRemove = existingProperty.Images.Where(image => !property.Images.Any(pImage => pImage.Id == image.Id)).ToList();
            _context.PropertyImages.RemoveRange(imagesToRemove);

            foreach (var image in property.Images)
            {
                if (!existingProperty.Images.Any(existingImage => existingImage.Id == image.Id))
                {
                    existingProperty.Images.Add(image);
                }
            }
        }
        _context.Properties.Update(existingProperty);
        await _context.SaveChangesAsync();
    }

    public async Task RemoveImages(IEnumerable<PropertyImage> images)
    {
        if (images != null && images.Any())
        {
            _context.PropertyImages.RemoveRange(images);
            await Task.CompletedTask;
        }
    }
}