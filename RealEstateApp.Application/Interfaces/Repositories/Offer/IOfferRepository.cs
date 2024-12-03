namespace RealEstateApp.Application.Interfaces.Repositories.Offer;
using Domain.Entities;
public interface IOfferRepository
{
    Task<IEnumerable<Offer>> GetOffersByUserAndPropertyAsync(string userId, int propertyId);
    Task<IEnumerable<string>> GetClientsByPropertyAsync(int propertyId);
    Task<bool> HasAcceptedOfferAsync(int propertyId);
    Task<bool> HasPendingOfferAsync(string userId, int propertyId);
    Task<Offer> GetByIdAsync(int offerId);
    Task AddAsync(Offer offer);
    Task SaveChangesAsync();
    Task<IEnumerable<Offer>> GetPendingOffersByPropertyAsync(int propertyId, int excludeOfferId);
}