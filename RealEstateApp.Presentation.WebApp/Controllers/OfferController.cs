using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Application.Interfaces.Services.Offer;
using RealEstateApp.Application.Interfaces.Services.Users;
using RealEstateApp.Application.ViewModels.Offer;
using RealEstateApp.Domain.Entities;
using RealEstateApp.Infrastructure.Identity.Entities;

namespace WebApplication1.Controllers;


public class OfferController : Controller
{
    private readonly IOfferService _offerService;
    private readonly UserManager<ApplicationUser> _userManager; 

    public OfferController(IOfferService offerService,  UserManager<ApplicationUser> userManager)
    {
        _offerService = offerService;
        _userManager = userManager;
    }

    [Authorize(Policy = "ClientOnly")]
    public async Task<IActionResult> ClientOffers(int propertyId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var offers = await _offerService.GetOffersByUserAndPropertyAsync(userId, propertyId);

        ViewBag.PropertyId = propertyId;
        return View("ClientOffers", offers);
    }
    
    [Authorize(Policy = "ClientOnly")]
    public async Task<IActionResult> CreateOffer(int propertyId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var canCreate = await _offerService.CanCreateOfferAsync(userId, propertyId);

        if (!canCreate)
        {
            TempData["Error"] = "You cannot create an offer for this property.";
            return RedirectToAction(nameof(ClientOffers), new { propertyId });
        }

        ViewBag.PropertyId = propertyId;
        return View("CreateOffer");
    }
    
    [HttpPost]
    [Authorize(Policy = "ClientOnly")]
    public async Task<IActionResult> CreateOffer(int propertyId, decimal amount)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var offer = new Offer
        {
            Amount = amount,
            PropertyId = propertyId,
            ClientId = userId
        };

        try
        {
            await _offerService.CreateOfferAsync(offer);
            TempData["Success"] = "Offer created successfully.";
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
        }

        return RedirectToAction(nameof(ClientOffers), new { propertyId });
    }
    
    [Authorize(Policy = "AgentOnly")]
  public async Task<IActionResult> AgentOffers(int propertyId)
  {

      var clientIds = await _offerService.GetClientsByPropertyAsync(propertyId);
  

      var clientsWithNames = new List<ClientViewModel>();
  
      foreach (var clientId in clientIds)
      {

          var client = await _userManager.FindByIdAsync(clientId); 
  
          if (client != null)
          {
   
              clientsWithNames.Add(new ClientViewModel
              {
                  ClientId = clientId,
                  ClientName = client.FirstName + client.LastName 
              });
          }
      }
  
      ViewBag.PropertyId = propertyId;
      return View("AgentOffers", clientsWithNames);
  }

    [Authorize(Policy = "AgentOnly")]
    public async Task<IActionResult> ClientOffersForAgent(int propertyId, string clientId)
    {
        var offers = await _offerService.GetOffersByUserAndPropertyAsync(clientId, propertyId);

        ViewBag.PropertyId = propertyId;
        ViewBag.ClientId = clientId;
        return View("ClientOffersForAgent", offers);
    }

    [HttpPost]
    [Authorize(Policy = "AgentOnly")]
    public async Task<IActionResult> RespondToOffer(int offerId, bool isAccepted)
    {
        try
        {
            await _offerService.RespondToOfferAsync(offerId, isAccepted);
            TempData["Success"] = isAccepted ? "Offer accepted successfully." : "Offer rejected.";
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
        }

        return RedirectToAction(nameof(AgentOffers));
    }
}