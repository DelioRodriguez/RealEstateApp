using RealEstateApp.Application.Interfaces.Repositories.Api;
using RealEstateApp.Domain.Entities;
using RealEstateApp.Infrastructure.Persistance.Context;

namespace RealEstateApp.Infrastructure.Persistance.Repositories.Api
{
    public class SaleTypeApiRepository : Repository<SaleType>, ISaleTypeApiRepository
    {
        private readonly AppDbContext _context;

        public SaleTypeApiRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
