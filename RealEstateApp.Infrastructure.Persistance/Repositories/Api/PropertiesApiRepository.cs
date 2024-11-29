using Microsoft.EntityFrameworkCore;
using RealEstateApp.Application.Interfaces.Repositories.Api;
using RealEstateApp.Domain.Entities;
using RealEstateApp.Infrastructure.Persistance.Context;

namespace RealEstateApp.Infrastructure.Persistance.Repositories.Api
{
    public class PropertiesApiRepository : Repository<Property>, IPropertiesApiRepository
    {
        private readonly AppDbContext _context;

        public PropertiesApiRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Property>> GetAllPropertiesAsync()
        {
            return await _context.Properties
                .Include(p => p.PropertyType)
                .Include(p => p.SaleType)
                .Include(p => p.Improvements)
                .ToListAsync();
        }

        public async Task<Property> GetByIdWithImprovementsAsync(int Id)
        {
            return await _context.Properties
                .Include(p => p.PropertyType)
                .Include(p => p.SaleType)
                .Include(p => p.Improvements)
                .FirstOrDefaultAsync(p => p.Id == Id);
        }

        public async Task<Property?> GetByCodeAsync(string code)
        {
            return await _context.Properties
            .Include(p => p.PropertyType)
            .Include(p => p.SaleType)
            .Include(p => p.Improvements)
            .FirstOrDefaultAsync(p => p.Code == code);
        }
    }
}
