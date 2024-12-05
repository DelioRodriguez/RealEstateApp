using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Application.Dtos.Account;
using RealEstateApp.Application.Interfaces.Services.Account;
using RealEstateApp.Application.Interfaces.Services.Properties;
using RealEstateApp.Application.Interfaces.Services.Users;

namespace WebApplication1.Controllers;

[Authorize(Policy = "AgentOnly")] 
public class AgentController : Controller
{
    private readonly IPropertyService _propertyService;
    private readonly IUserService _userService;
    private readonly IAccountService _accountService;

    public AgentController(IPropertyService propertyService, IUserService userService, IAccountService accountService)
    {
        _propertyService = propertyService;
        _userService = userService;
        _accountService = accountService;
    }

    public async Task<IActionResult> MyProperties()
    {
        return View(await _propertyService.GetAllPropertiesByUserAsync(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value!));
    }

    public async Task<IActionResult> MantenimientoPropiedades()
    {
        return View(await _propertyService.GetAllPropertyByUserIdAsync(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value!));
    }

    public async Task<IActionResult> Profile()
    {
        return View(await _userService.GetUserByIdDto(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value!) );
    }

    [Authorize(Roles = "Agent")]
    [HttpPost]
    public async Task<IActionResult> Profile(UserUpdateDTO userDto)
    {
        if (!ModelState.IsValid)
        {
            var errorMessage = string.Join(" ", ModelState.Values
                .Where(v => v.Errors.Count > 0)
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage));
            
            TempData["ErrorMessage"] = errorMessage;
            return RedirectToAction("Profile", userDto);
        }

        var result = await _accountService.UpdateUserAsync(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value, userDto);

        if (result.Contains("successfully"))
        {
            TempData["SuccessMessage"] = result;
            return RedirectToAction("Profile");
        }
        
        TempData["ErrorMessage"] = result;
        return View(userDto);
    }
}