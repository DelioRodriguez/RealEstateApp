using RealEstateApp.Application.Interfaces.Repositories.Api;
using RealEstateApp.Domain.Entities;
using RealEstateApp.Infrastructure.Persistance.Context;

namespace RealEstateApp.Infrastructure.Persistance.Repositories.Api
{
    public class ImprovementsApiRepository : Repository<Improvement>, IImprovementsApiRepository
    {
        private readonly AppDbContext _context;
        public ImprovementsApiRepository(AppDbContext appDbContext) : base(appDbContext) 
        {
            _context = appDbContext;
        }
    }
}
