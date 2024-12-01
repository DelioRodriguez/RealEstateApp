using Microsoft.EntityFrameworkCore;
using RealEstateApp.Application.Interfaces.Repositories.PropertiesType;
using RealEstateApp.Domain.Entities;
using RealEstateApp.Infrastructure.Persistance.Context;

namespace RealEstateApp.Infrastructure.Persistance.Repositories.PropertiesType;

public class PropertyTypeRepository : Repository<PropertyType>, IPropertyTypeRepository
{
    private readonly AppDbContext _context;

    public PropertyTypeRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<int> GetPropertiesCountAsync(int propertyTypeId)
    {
        return await _context.Properties.CountAsync(p  => p.PropertyTypeId == propertyTypeId);
    }
}