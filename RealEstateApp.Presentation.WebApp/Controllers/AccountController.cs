using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Application.Dtos.Account;
using RealEstateApp.Application.Interfaces.Services.Account;

namespace WebApplication1.Controllers;


public class AccountController : Controller
{
    private readonly IAccountService _accountService;

    public AccountController(IAccountService accountService)
    {
        _accountService = accountService;
    }
    public IActionResult Register()
    {
        var model = new UserRegisterDTO();
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(UserRegisterDTO userDto)
    {
        if (!ModelState.IsValid)
            return View(userDto);

        if (userDto.Photo.Length > 0)
        {
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
            Directory.CreateDirectory(uploadsFolder);

            var uniqueFileName = $"{Guid.NewGuid()}_{userDto.Photo.FileName}";
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await userDto.Photo.CopyToAsync(stream);
            }
            userDto.ImagenPath = $"/uploads/{uniqueFileName}";
        }
        
        var result = await _accountService.RegisterUserAsync(userDto);

        if (result.Contains("succesful"))
        {
            TempData["SuccessMessage"] = result;
            return RedirectToAction("Login");
        }
        
        TempData["ErrorMessage"] = result;
        return View(userDto);
    }

    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(UserLoginDTO userDto)
    {
        if (!ModelState.IsValid)
        {
            return View(userDto);
        }
        
        var result = await _accountService.LoginUserAsync(userDto);

        if (result.Contains("successful"))
        {
            TempData["SuccessMessage"] = result;
            return RedirectToAction("Index", "Properties");
        }
        TempData["ErrorMessage"] = result;
        return View(userDto);
    }

    [AllowAnonymous]
    public async Task<IActionResult> Activate(string email, string token)
    {
        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(token))
        {
            TempData["ErrorMessage"] = "El correo electrónico o el token están vacíos.";
            return RedirectToAction("Login");
        }

        var activationSuccess = await _accountService.ActivateUserAsync(email, token);

        if (!activationSuccess)
        {
            TempData["ErrorMessage"] = "La activación falló. Verifique el correo electrónico o el token.";
            return RedirectToAction("Login");
        }

        TempData["SuccessMessage"] = "¡Tu cuenta ha sido activada con éxito!";
        return View();
    }


     [Authorize]
     public async Task<IActionResult> Logout()
     {
         await _accountService.LogoutAsync();
         return RedirectToAction("index", "Properties");
     }
}