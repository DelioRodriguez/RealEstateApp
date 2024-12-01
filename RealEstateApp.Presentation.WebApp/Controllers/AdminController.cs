using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Application.Dtos.Admin;
using RealEstateApp.Application.Interfaces.Services.Admin;
using RealEstateApp.Application.Interfaces.Services.Agent;
using RealEstateApp.Application.Interfaces.Services.Dashboard;

namespace WebApplication1.Controllers;
[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly IAdminService _adminService;
    private readonly IDashboardService _dashboardService;
    private readonly IAgentService _agentService;

    public AdminController(IDashboardService dashboardService, IAgentService agentService, IAdminService adminService)
    {
        _dashboardService = dashboardService;
        _agentService = agentService;
        _adminService = adminService;
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

    public async Task<IActionResult> Admins()
    {
        var admins = await _adminService.GetAllAdminsAsync();
        return View(admins);
    }

    public IActionResult CreateAdmin()
    {
        return View();
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateAdmin(AdminDto adminDto)
    {
        if (!ModelState.IsValid)
        {
            return View(adminDto); 
        }

        var result = await _adminService.CreateAdminAsync(adminDto);
        if (!result)
        {
            ModelState.AddModelError("", "No se pudo crear el administrador.");
            return View(adminDto);
        }

        return RedirectToAction(nameof(Admins));
    }
    
    
    public async Task<IActionResult> EditAdmin(string id)
    {
        var admin = await _adminService.GetAdminByIdAsync(id);
        if (admin == null)
        {
            return NotFound(); 
        }

        return View(admin); 
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditAdmin(string id, AdminDto adminDto)
    {
        if (id != adminDto.Id)
        {
            return BadRequest(); 
        }

        if (!ModelState.IsValid)
        {
            return View(adminDto);
        }

        var result = await _adminService.UpdateAdminAsync(adminDto);
        if (!result)
        {
            ModelState.AddModelError("", "No se pudo actualizar el administrador.");
            return View(adminDto);
        }

        return RedirectToAction(nameof(Admins)); 
    }
    
    public async Task<IActionResult> ToggleStatusAdmin(string id, bool isActive)
    {
        var result = await _adminService.ToggleAdminStatusAsync(id, isActive);
        if (!result)
        {
            TempData["Error"] = "No se pudo cambiar el estado del administrador.";
        }
        else
        {
            TempData["Success"] = $"Estado del administrador cambiado a {(isActive ? "activo" : "inactivo")}.";
        }

        return RedirectToAction(nameof(Admins));
    }


}