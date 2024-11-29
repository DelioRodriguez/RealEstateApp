using RealEstateApp.Application.Interfaces.Repositories.Api;
using RealEstateApp.Domain.Entities;
using RealEstateApp.Infrastructure.Persistance.Context;

namespace RealEstateApp.Infrastructure.Persistance.Repositories.Api
{
    public class PropertyTypesApiRepository : Repository<PropertyType>, IPropertyTypesApiRepository
    {
        private readonly AppDbContext _context;

        public PropertyTypesApiRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }


    }
}
