using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Application.Dtos.ApiAccount;
using RealEstateApp.Application.Interfaces.Services.Account;
using RealEstateApp.Domain.Enums;

namespace RealEstateApp.Presentation.Api5.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IConfiguration _configuration;

        public AccountController(IAccountService accountService, IConfiguration configuration)
        {
            _accountService = accountService;
            _configuration = configuration;
        }


        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            var response = await _accountService.LoginAsync(request);

            if (!response.Success)
            {
                var errorMessage = response.Errors?.FirstOrDefault() ?? "Invalid login attempt.";
                return Unauthorized(new { message = errorMessage });
            }

            return Ok(new
            {
                Success = response.Success,
                UserId = response.UserId,
                UserName = response.UserName,
                Email = response.Email,
                Role = response.Role,
                Token = response.Token
            });
        }


        [Authorize(Roles = "Admin")]
        [HttpPost("register/admin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterRequestDto request)
        {
            var response = await _accountService.RegisterAsync(request, Role.Admin.ToString());

            if (!response.Success)
            {
                return BadRequest(new { errors = response.Errors });
            }

            return Ok(new
            {
                Success = response.Success,
                UserId = response.UserId,
                UserName = response.UserName,
                Email = response.Email
            });
        }

        [Authorize(Roles = "Developer,Admin")]
        [HttpPost("register/developer")]
        public async Task<IActionResult> RegisterDeveloper([FromBody] RegisterRequestDto request)
        {
            var response = await _accountService.RegisterAsync(request, Role.Developer.ToString());

            if (!response.Success)
            {
                return BadRequest(new { errors = response.Errors });
            }

            return Ok(new
            {
                Success = response.Success,
                UserId = response.UserId,
                UserName = response.UserName,
                Email = response.Email
            });
        }
    }
}