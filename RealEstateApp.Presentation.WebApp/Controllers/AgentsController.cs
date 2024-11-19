using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Application.Enums;
using RealEstateApp.Application.Interfaces.Services.Users;
using RealEstateApp.Application.ViewModels.Users;

namespace WebApplication1.Controllers;

public class AgentsController : Controller
{
    private readonly IUserService _userService;

    public AgentsController(IUserService userService)
    {
        _userService = userService;
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
}