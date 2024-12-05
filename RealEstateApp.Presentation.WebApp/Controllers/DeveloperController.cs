using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Application.Dtos.Developer;
using RealEstateApp.Application.Interfaces.Services.Developer;

namespace WebApplication1.Controllers;

[Authorize(Policy = "AdminOnly")] 

public class DeveloperController : Controller
{
    private readonly iDeveloperService _developerService;

    public DeveloperController(iDeveloperService developerService)
    {
        _developerService = developerService;
    }

    public async Task<IActionResult> Index()
    {
        var developers = await _developerService.GetAllDevelopersAsync();
        return View(developers);
    }

    public IActionResult Create()
    {
        return View(new DeveloperDto());
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(DeveloperDto developerDto)
    {
        if (!ModelState.IsValid)
        {
            return View(developerDto);
        }
        
        var result = await _developerService.CreateDeveloperAsync(developerDto);
        if (!result)
        {
            ModelState.AddModelError("","No se pudo crear el desarrollador");
            return View(developerDto);
        }
        return RedirectToAction("Index");
    }
    
    public async Task<IActionResult> Edit(string id)
    {
        var developer = await _developerService.GetDeveloperByIdAsync(id);
        if (developer == null)
        {
            return NotFound();
        }

        return View(developer);
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(DeveloperDto developerDto)
    {
        if (!ModelState.IsValid)
        {
            return View(developerDto);
        }

        var result = await _developerService.UpdateDeveloperAsync(developerDto);
        if (!result)
        {
            ModelState.AddModelError("", "No se pudo actualizar el desarrollador.");
            return View(developerDto);
        }

        return RedirectToAction(nameof(Index));
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ToggleStatus(string id, bool status)
    {
        var result = await _developerService.ToggleDeveloperStatusAsync(id, status);
      return RedirectToAction(nameof(Index));
    }
}