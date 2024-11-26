using RealEstateApp.Application.Interfaces.Repositories.Improvements;
using RealEstateApp.Domain.Entities;
using RealEstateApp.Infrastructure.Persistance.Context;

namespace RealEstateApp.Infrastructure.Persistance.Repositories.Improvements;

public class ImprovementRepository : Repository<Improvement>, IImprovementRepository
{
    public ImprovementRepository(AppDbContext context) : base(context)
    {
    }
}