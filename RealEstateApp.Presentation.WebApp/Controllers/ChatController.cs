using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers;

public class ChatController : Controller
{
    public IActionResult DetailsByAgent()
    {
        return View();
    }
}