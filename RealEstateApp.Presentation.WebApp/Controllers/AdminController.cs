using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Application.Interfaces.Services.Agent;
using RealEstateApp.Application.Interfaces.Services.Dashboard;

namespace WebApplication1.Controllers;

public class AdminController : Controller
{
    private readonly IDashboardService _dashboardService;
    private readonly IAgentService _agentService;

    public AdminController(IDashboardService dashboardService, IAgentService agentService)
    {
        _dashboardService = dashboardService;
        _agentService = agentService;
    }

    public async Task<IActionResult> Index()
    {
        var dashboardData = await _dashboardService.GetDashboardDataAsync();
        return View(dashboardData);
    }

    public async Task<IActionResult> Agents()
    {
        var agents = await _agentService.GetAgentsAsync();
        return View(agents);
    }

    [HttpPost]
    public async Task<IActionResult> ToggleActivation(string id)
    {
        await _agentService.ToggleAgentActivationAsync(id);
        return RedirectToAction(nameof(Agents));
    }
    
    [HttpPost]
    public async Task<IActionResult> Delete(string id)
    {
        await _agentService.DeleteAgentAsync(id);
        return RedirectToAction(nameof(Agents));
    }
    
}