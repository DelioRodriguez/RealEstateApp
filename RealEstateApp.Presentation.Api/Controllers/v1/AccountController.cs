using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Application.Dtos.ApiAccount;
using RealEstateApp.Application.Interfaces.Services.Account;
using RealEstateApp.Domain.Enums;
using Swashbuckle.AspNetCore.Annotations;
using System.Net.Mime;

namespace RealEstateApp.Presentation.Api5.Controllers.v1
{
    [ApiVersion("1.0")]
    [SwaggerTag("Account Controller")]
    public class AccountController : BaseApiController
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
        [SwaggerOperation(
            Summary = "Iniciar seccion",
            Description = "Inicio de seccion para obtener el JWT y utilizar las demas funcionalidades de la Api.")]
        // Tipo de contenido que menejan
        [Consumes(MediaTypeNames.Application.Json)] 
        [Produces(MediaTypeNames.Application.Json)]
        // Estados que retornan los metodos
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
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
        [SwaggerOperation(
            Summary = "Registrar un nuevo administrador",
            Description = "Permite registrar un nuevo usuario como admin.")]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
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
        [SwaggerOperation(
            Summary = "Registrar un nuevo desarrollador",
            Description = "Permite registrar un nuevo usuario como desarrollador.")]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
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