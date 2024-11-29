using Microsoft.EntityFrameworkCore;
using RealEstateApp.Application.Interfaces.Repositories.Improvements;
using RealEstateApp.Domain.Entities;
using RealEstateApp.Infrastructure.Persistance.Context;

namespace RealEstateApp.Infrastructure.Persistance.Repositories.Improvements;

public class ImprovementRepository : Repository<Improvement>, IImprovementRepository
{
    private readonly AppDbContext _context;
    
    public ImprovementRepository(AppDbContext context) : base(context)
    {
        this._context = context;
    }

    public async Task<List<Improvement>> GetImprovementsByPropertyIdAsync(int propertyId)
    {
        return await _context.Improvements
            .Where(i => i.Properties.Any(p => p.Id == propertyId)).ToListAsync();
    }
}