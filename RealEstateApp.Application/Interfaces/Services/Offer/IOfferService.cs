namespace RealEstateApp.Application.Interfaces.Services.Offer;
using RealEstateApp.Domain.Entities;
public interface IOfferService
{
    Task<IEnumerable<Offer>> GetOffersByUserAndPropertyAsync(string userId, int propertyId);
    Task<IEnumerable<string>> GetClientsByPropertyAsync(int propertyId);
    Task<bool> CanCreateOfferAsync(string userId, int propertyId);
    Task CreateOfferAsync(Offer offer);
    Task RespondToOfferAsync(int offerId, bool isAccepted);
}