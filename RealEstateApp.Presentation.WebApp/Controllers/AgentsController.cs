using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Application.Interfaces.Services.Improvements;
using RealEstateApp.Application.Interfaces.Services.Properties;
using RealEstateApp.Application.Interfaces.Services.Users;
using RealEstateApp.Application.ViewModels.Users;
using RealEstateApp.Domain.Enums;

namespace WebApplication1.Controllers;

public class AgentsController : Controller
{
    private readonly IUserService _userService;
    private readonly IPropertyService _propertyService;
    private readonly IImprovementService _improvementService;

    public AgentsController(IUserService userService, IPropertyService propertyService, IImprovementService improvementService)
    {
        _userService = userService;
        _propertyService = propertyService;
        _improvementService = improvementService;
    }

    public async Task<IActionResult> Agents(string searchName)
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

    public async Task<IActionResult> PropertiesByAgent(string id)
    {
        return View(await _propertyService.GetPropertyByUserIdAsync(id));
    }

    public async Task<IActionResult> Details(int id)
    {
        var property = await _propertyService.GetPropertyDetailsAsync(id);
        property.Improvements = await _improvementService.GetImprovementsByPropertyIdAsync(id);
        
        return View(property);
    }
    
}