using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Domain.Enums;

namespace WebApplication1.Controllers;

public class HomeController : Controller
{

    public IActionResult RedirectByRole()
    {
        if (User.IsInRole(Role.Admin.ToString()))
        {
            return RedirectToAction("Index", "Admin");
        }

        return RedirectToAction("Index", "Properties");
    }
    
}