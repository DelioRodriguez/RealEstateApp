using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Application.Interfaces.Services.Favory;
using RealEstateApp.Application.Interfaces.Services.Improvements;
using RealEstateApp.Application.Interfaces.Services.Properties;
using RealEstateApp.Application.Interfaces.Services.Users;
using RealEstateApp.Application.ViewModels.Properties;
using RealEstateApp.Application.ViewModels.Users;
using RealEstateApp.Domain.Enums;

namespace WebApplication1.Controllers;

public class ClientController : Controller
{
    private readonly IPropertyService  _propertyService;
    private readonly IFavoriteService  _favoriteService;
    private readonly IUserService  _userService;
    private readonly IImprovementService _improvementService;

    public ClientController(IPropertyService propertyService, IFavoriteService favoriteService, IUserService userService, IImprovementService improvementService)
    {
        _propertyService = propertyService;
        _favoriteService = favoriteService;
        _userService = userService;
        _improvementService = improvementService;
    }
    
    public async Task<IActionResult> AgentsByClient(string searchName)
    {
        IEnumerable<AgentViewModel> users;
        if (string.IsNullOrEmpty(searchName))
        {
            users = await _userService.GetUsersByRoleAsync(Role.Agent.ToString());
        }
        else
        {
            users = await _userService.GetAgentsByNameAsync(searchName);
        }

        return View(users);
    }
    
    public async Task<IActionResult> PropertiesAgentsByClient(string id)
    {
        return View(await _propertyService.GetPropertyByUserIdAsync(id));
    }

    public async Task<IActionResult> DetailsWithChat(int id)
    {
        var property = await _propertyService.GetPropertyDetailsAsync(id);
        property.Improvements = await _improvementService.GetImprovementsByPropertyIdAsync(id);
        
        return View(property);
    }
    
    public async Task<IActionResult> HomeClient()
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        
        var properties = await _propertyService.GetAvailablePropertiesAsync();
        
        if (!string.IsNullOrEmpty(userId))
        {
            var favoriteIds = await _favoriteService.GetFavoritePropertyIdsAsync(userId);
            
            foreach (var property in properties)
            {
                property.IsFavorite = favoriteIds.Any(favId => favId == property.Id);
            }
            
            properties = properties
                .OrderByDescending(x => x.IsFavorite)
                .ThenBy(x => x.Price)
                .Distinct()
                .ToList();
        }
        
        return View(properties);
    }
    
    [HttpPost]
    public async Task<IActionResult> HomeClient(PropertyFilterViewModel? filter)
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
    
        var properties = await _propertyService.GetAvailablePropertiesAsync();
    
        if (!string.IsNullOrEmpty(userId))
        {
            var favoriteIds = await _favoriteService.GetFavoritePropertyIdsAsync(userId);
        
            foreach (var property in properties)
            {
                property.IsFavorite = favoriteIds.Any(favId => favId == property.Id);
            }
            
            properties = properties
                .OrderByDescending(x => x.IsFavorite) 
                .ThenBy(x => x.Price)
                .Distinct()
                .ToList();
        }

        if (filter != null)
        {
            properties = await _propertyService.SearchPropertiesAsync(filter);

            if (!string.IsNullOrEmpty(userId))
            {
                var favoriteIds = await _favoriteService.GetFavoritePropertyIdsAsync(userId);
            
                foreach (var property in properties)
                {
                    property.IsFavorite = favoriteIds.Any(favId => favId == property.Id);
                }
            }

            properties = properties
                .OrderByDescending(x => x.IsFavorite)
                .ThenBy(x => x.Price)
                .Distinct()
                .ToList();
        }

        return View(properties);
    }

}