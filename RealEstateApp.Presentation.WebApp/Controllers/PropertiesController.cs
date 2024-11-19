using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Application.Interfaces.Services.Properties;
using RealEstateApp.Application.ViewModels.Properties;

namespace WebApplication1.Controllers;

public class PropertiesController : Controller
{
    private readonly IPropertyService  _propertyService;

    public PropertiesController(IPropertyService propertyService)
    {
        this._propertyService = propertyService;
    }

    public async Task<IActionResult> Index()
    {
        var properties = await _propertyService.GetAvailablePropertiesAsync();
        return View(properties);
    }

    [HttpPost]
    public async Task<IActionResult> Index(PropertyFilterViewModel filter)
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
}