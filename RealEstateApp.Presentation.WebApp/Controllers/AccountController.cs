using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Application.Dtos.Account;
using RealEstateApp.Application.Helpers;
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

        userDto.ImagenPath = await FileHelper.SaveImageAsync(userDto.Photo);

        await _accountService.RegisterUserAsync(userDto);
        return RedirectToAction("Login");
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
            return View(userDto);

        var loginResult = await _accountService.LoginUserAsync(userDto);

        if (!loginResult.IsSuccess)
        {
            ModelState.AddModelError(string.Empty, "Credenciales inválidas.");
            return View(userDto);
        }

        if (loginResult.IsDeveloper) 
        {
            ModelState.AddModelError(string.Empty, "No tienes acceso a la app web.");
            return View(userDto);
        }

        return loginResult.IsAdmin 
            ? RedirectToAction("Index", "Admin") 
            : RedirectToAction("Index", "Properties");
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