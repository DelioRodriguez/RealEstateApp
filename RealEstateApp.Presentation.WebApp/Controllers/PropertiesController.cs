using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Application.Interfaces.Services.Favory;
using RealEstateApp.Application.Interfaces.Services.Properties;
using RealEstateApp.Application.ViewModels.Properties;

namespace WebApplication1.Controllers;

public class PropertiesController : Controller
{
    private readonly IPropertyService  _propertyService;
    private readonly IFavoriteService  _favoriteService;

    public PropertiesController(IPropertyService propertyService, IFavoriteService favoriteService)
    {
        this._propertyService = propertyService;
        _favoriteService = favoriteService;
    }

    public async Task<IActionResult> Index()
    {
        if (User.Identity!.IsAuthenticated)
        {
            return RedirectToAction("HomeClient", "Client");
        }
        else
        {
            var properties = await _propertyService.GetAvailablePropertiesAsync();
            return View(properties);
        }
    }

    [HttpPost]
    public async Task<IActionResult> Index(PropertyFilterViewModel? filter)
    {
        var properties = await _propertyService.SearchPropertiesAsync(filter);
        
        properties = properties.OrderBy(x => x.Price).ToList();
        
        foreach (var property in properties)
        {
            property.Filter = filter; 
        }
            
        return View(properties);
    }
    
    public async Task<IActionResult> Details(int id)
    {
        var property = await _propertyService.GetPropertyDetailsAsync(id);

        return View(property); 
    }

    [HttpPost]
    public async Task<IActionResult> ToggleFavorite(int propertyId)
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }
        var success = await _favoriteService.ToggleFavoriteAsync(propertyId, userId);

        if (!success)
        {
            TempData["Error"] = "No se pudo actualizar el estado de favorito.";
        }

        return RedirectToAction(nameof(Index));
    }
    
    public async Task<IActionResult> Favorites()
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        } 
        await _favoriteService.GetFavoritePropertiesAsync(userId);

        return RedirectToAction("HomeClient", "Client");
    }
}