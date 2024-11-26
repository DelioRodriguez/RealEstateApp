using RealEstateApp.Application.Interfaces.Repositories.PropertiesType;
using RealEstateApp.Domain.Entities;
using RealEstateApp.Infrastructure.Persistance.Context;

namespace RealEstateApp.Infrastructure.Persistance.Repositories.PropertiesType;

public class PropertyTypeRepository : Repository<PropertyType>, IPropertyTypeRepository
{
    public PropertyTypeRepository(AppDbContext context) : base(context)
    {
    }
}