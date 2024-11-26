using RealEstateApp.Application.Interfaces.Repositories.SalesType;
using RealEstateApp.Domain.Entities;
using RealEstateApp.Infrastructure.Persistance.Context;

namespace RealEstateApp.Infrastructure.Persistance.Repositories.SalesType;

public class SaleTypeRepository : Repository<SaleType>, ISaleTypeRepository
{
    private readonly AppDbContext _context;
    
    public SaleTypeRepository(AppDbContext context) : base(context)
    {
        this._context = context;
    }
}