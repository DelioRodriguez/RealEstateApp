using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Application.Interfaces.Services.Properties;

namespace WebApplication1.Controllers;

[Authorize(Roles = "Agent")]
public class AgentController : Controller
{
    private readonly IPropertyService _propertyService;

    public AgentController(IPropertyService propertyService)
    {
        _propertyService = propertyService;
    }

    public async Task<IActionResult> MyProperties()
    {
        return View(await _propertyService.GetAllPropertiesByUserAsync(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value!));
    }

    public async Task<IActionResult> MantenimientoPropiedades()
    {
        return View(await _propertyService.GetAvailablePropertiesAsync());
    }
}