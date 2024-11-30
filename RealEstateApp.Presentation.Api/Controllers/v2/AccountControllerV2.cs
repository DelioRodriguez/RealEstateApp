using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Application.Dtos.ApiAccount;
using RealEstateApp.Application.Interfaces.Services.Account;
using Swashbuckle.AspNetCore.Annotations;
using System.Net.Mime;

namespace RealEstateApp.Presentation.Api5.Controllers.v2
{
    [ApiVersion("2.0")]
    [Authorize(Roles = "Admin")]
    public class AccountControllerV2 : BaseApiController
    {

        private readonly IAccountService _accountService;
        private readonly IConfiguration _configuration;

        public AccountControllerV2(IAccountService accountService, IConfiguration configuration)
        {
            _accountService = accountService;
            _configuration = configuration;
        }


        [HttpGet("test")]
        public IActionResult TestV2()
        {
            return Ok("API Version 2 is working");
        }

        [AllowAnonymous]
        [HttpPost("login")]
        [SwaggerOperation(
            Summary = "Iniciar seccion",
            Description = "Inicio de seccion para obtener el JWT y utilizar las demas funcionalidades de la Api."
        )]
        [Consumes(MediaTypeNames.Application.Json)]
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
    }
}
