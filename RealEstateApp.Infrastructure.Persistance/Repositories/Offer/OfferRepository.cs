using Microsoft.EntityFrameworkCore;
using RealEstateApp.Application.Interfaces.Repositories.Offer;
using RealEstateApp.Domain.Enums;
using RealEstateApp.Infrastructure.Persistance.Context;

namespace RealEstateApp.Infrastructure.Persistance.Repositories.Offer;

public class OfferRepository: IOfferRepository
{
    private readonly AppDbContext _context;

    public OfferRepository(AppDbContext context)
    {
        _context = context;
    }


    public async Task<IEnumerable<Domain.Entities.Offer>> GetOffersByUserAndPropertyAsync(string userId, int propertyId)
    {
        return await _context.Offers
            .Where(o => o.ClientId == userId && o.PropertyId == propertyId)
            .ToListAsync();
    }

    public async Task<IEnumerable<string>> GetClientsByPropertyAsync(int propertyId)
    {
        return await _context.Offers
            .Where(o => o.PropertyId == propertyId)
            .Select(o => o.ClientId)
            .Distinct()
            .ToListAsync();
    }

    public async Task<bool> HasAcceptedOfferAsync(int propertyId)
    {
        return await _context.Offers
            .AnyAsync(o => o.PropertyId == propertyId && o.Status == Status.Accepted);
    }

    public async Task<bool> HasPendingOfferAsync(string userId, int propertyId)
    {
        return await _context.Offers
            .AnyAsync(o => o.PropertyId == propertyId && o.Status == Status.Pending);
    }

    public async Task<Domain.Entities.Offer> GetByIdAsync(int offerId)
    {
        return await _context.Offers.FindAsync(offerId);
    }

    public async Task AddAsync(Domain.Entities.Offer offer)
    {
        await _context.Offers.AddAsync(offer);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Domain.Entities.Offer>> GetPendingOffersByPropertyAsync(int propertyId, int excludeOfferId)
    {
        return await _context.Offers
            .Where(o => o.PropertyId == propertyId && o.Status == Status.Pending && o.Id != excludeOfferId)
            .ToListAsync();
    }
}