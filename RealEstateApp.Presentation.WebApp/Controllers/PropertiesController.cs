using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Application.Exceptions;
using RealEstateApp.Application.Interfaces.Services.Favory;
using RealEstateApp.Application.Interfaces.Services.Improvements;
using RealEstateApp.Application.Interfaces.Services.Properties;
using RealEstateApp.Application.ViewModels.Properties;

namespace WebApplication1.Controllers;

public class PropertiesController : Controller
{
    private readonly IPropertyService  _propertyService;
    private readonly IFavoriteService  _favoriteService;
    private readonly IImprovementService _improvementService;

    public PropertiesController(IPropertyService propertyService, IFavoriteService favoriteService, IImprovementService improvementService)
    {
        this._propertyService = propertyService;
        _favoriteService = favoriteService;
        _improvementService = improvementService;
    }

    public async Task<IActionResult> Index()
    {
        if (User.Identity!.IsAuthenticated && User.IsInRole("Client"))
        {
            return RedirectToAction("HomeClient", "Client");
        }
        else if (User.Identity.IsAuthenticated && User.IsInRole("Agent"))
        {
            return RedirectToAction("MyProperties", "Agent");
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

        property.Improvements = await _improvementService.GetImprovementsByPropertyIdAsync(id);

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
    
    
    public async Task<IActionResult> CreateProperties()
    {
        var viewModel = await _propertyService.GetCreatePropertyViewModelAsync();
        return View(viewModel);
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateProperties(PropertyCreateViewModel model)
    {
        model.AgentId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        
        try
        {
            await _propertyService.AddPropertyAsync(model);
            TempData["SuccessMessage"] = "Propiedad creada exitosamente.";
            return RedirectToAction("MantenimientoPropiedades", "Agent");
        }
        catch (ValidationException ex)
        {
            TempData["ErrorMessage"] = ex.Message;
            return RedirectToAction("CreateProperties");
        }
        catch (DuplicateException ex)
        {
            TempData["ErrorMessage"] = ex.Message;
            return RedirectToAction("CreateProperties");
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = ex.Message;
            return RedirectToAction("CreateProperties");
        }
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _propertyService.DeletePropertyAsync(id);
        
        if (result == 0)
        {
            TempData["ErrorMessage"] = "No se pudo eliminar el propuesto.";
            return View("CreateProperties");
        }
        
        TempData["SuccessMessage"] = "Propuesto eliminado exitosamente.";
        return RedirectToAction("MantenimientoPropiedades", "Agent");
    }
    
    
    public IActionResult Edit(int id)
    {
        var property = _propertyService.GetByIdAsync(id); 
    
        return View("EditProperties");
    }
}