using Microsoft.EntityFrameworkCore;
using RealEstateApp.Application.Interfaces.Repositories.SalesType;
using RealEstateApp.Domain.Entities;
using RealEstateApp.Infrastructure.Persistance.Context;

namespace RealEstateApp.Infrastructure.Persistance.Repositories.SalesType;

public class SaleTypeRepository : Repository<SaleType>, ISaleTypeRepository
{
    private readonly AppDbContext _context;
    
    public SaleTypeRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<int> GetPropertiesCountAsync(int saleTypeId)
    {
        return await _context.Properties.CountAsync(p => p.SaleTypeId == saleTypeId);
    }
}