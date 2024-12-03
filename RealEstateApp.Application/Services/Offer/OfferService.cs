using RealEstateApp.Application.Interfaces.Repositories.Offer;
using RealEstateApp.Application.Interfaces.Repositories.Properties;
using RealEstateApp.Application.Interfaces.Services.Offer;
using RealEstateApp.Domain.Enums;

namespace RealEstateApp.Application.Services.Offer;

public class OfferService : IOfferService
{
    private readonly IOfferRepository _offerRepository;
    private readonly IPropertyRepository _propertyRepository;

    public OfferService(IOfferRepository offerRepository, IPropertyRepository propertyRepository)
    {
        _offerRepository = offerRepository;
        _propertyRepository = propertyRepository;
    }

    public async Task<IEnumerable<Domain.Entities.Offer>> GetOffersByUserAndPropertyAsync(string userId, int propertyId)
    {
        return await _offerRepository.GetOffersByUserAndPropertyAsync(userId, propertyId);
    }

    public async Task<IEnumerable<string>> GetClientsByPropertyAsync(int propertyId)
    {
        return await _offerRepository.GetClientsByPropertyAsync(propertyId);
    }

    public async Task<bool> CanCreateOfferAsync(string userId, int propertyId)
    {
        var hasAcceptedOffer = await _offerRepository.HasAcceptedOfferAsync(propertyId);
        var hasPendingOffer = await _offerRepository.HasPendingOfferAsync(userId, propertyId);

        return !hasAcceptedOffer && !hasPendingOffer;
    }

    public async Task CreateOfferAsync(Domain.Entities.Offer offer)
    {
        if (offer == null)
            throw new ArgumentNullException(nameof(offer));

        var canCreate = await CanCreateOfferAsync(offer.ClientId, offer.PropertyId);
        if (!canCreate)
            throw new InvalidOperationException("Cannot create offer. Either an accepted offer exists or there is a pending offer.");

        offer.Status = Status.Pending;
        offer.Date = DateTime.UtcNow;

        await _offerRepository.AddAsync(offer);
        await _offerRepository.SaveChangesAsync();
    }

    public async Task RespondToOfferAsync(int offerId, bool isAccepted)
    {
        var offer = await _offerRepository.GetByIdAsync(offerId);
        if (offer == null)
            throw new KeyNotFoundException("Offer not found.");

        if (offer.Status != Status.Pending)
            throw new InvalidOperationException("Offer is not in a pending state.");

        if (isAccepted)
        {
            offer.Status = Status.Accepted;

            var pendingOffers = await _offerRepository.GetPendingOffersByPropertyAsync(offer.PropertyId, offerId);
            foreach (var pendingOffer in pendingOffers)
            {
                pendingOffer.Status = Status.Rejected;
            }

            var property = await _propertyRepository.GetByIdAsync(offer.PropertyId);
            if (property == null)
                throw new KeyNotFoundException("Property not found.");

            property.IsAvailable = false;
            await _propertyRepository.SaveChangesAsync();
        }
        else
        {
            offer.Status = Status.Rejected;
        }

        await _offerRepository.SaveChangesAsync();
    }
}
