using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Application.Interfaces.Services.Dashboard;

namespace WebApplication1.Controllers;


public class AdminController : Controller
{
    private readonly IDashboardService _dashboardService;

    public AdminController(IDashboardService dashboardService)
    {
        _dashboardService = dashboardService;
    }

    public async Task<IActionResult> Index()
    {
        var dashboardData = await _dashboardService.GetDashboardDataAsync();
        return View(dashboardData);
    }
}