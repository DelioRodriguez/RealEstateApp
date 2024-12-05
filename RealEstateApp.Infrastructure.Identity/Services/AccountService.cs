using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RealEstateApp.Application.Dtos.Account;
using RealEstateApp.Application.Dtos.ApiAccount;
using RealEstateApp.Application.Dtos.Login;
using RealEstateApp.Application.Exceptions;
using RealEstateApp.Application.Helpers;
using RealEstateApp.Application.Interfaces.Services.Account;
using RealEstateApp.Application.Settings;
using RealEstateApp.Domain.Enums;
using RealEstateApp.Infrastructure.Identity.Entities;
using RealEstateApp.Infrastructure.Shared.IService;

namespace RealEstateApp.Infrastructure.Identity.Services;

public class AccountService : IAccountService
{
    private readonly IEmailService _emailService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IMapper _mapper;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly HtmlEncoder _htmlEncoder;
    private readonly IConfiguration _configuration;

    public AccountService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, HtmlEncoder htmlEncoder, IMapper mapper, IHttpContextAccessor httpContextAccessor, IEmailService emailService, IConfiguration configuration)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _htmlEncoder = htmlEncoder;
        _mapper = mapper;
        _httpContextAccessor = httpContextAccessor;
        _emailService = emailService;
        _configuration = configuration;
    }
    
    
    public async Task RegisterUserAsync(UserRegisterDTO userDto)
    {
        if (userDto.Password != userDto.ConfirmPassword)
            throw new ValidationException("Passwords do not match.");
        
        var existingUserByEmail = await _userManager.FindByEmailAsync(userDto.Email!);
        if (existingUserByEmail != null)
            throw new ValidationException("Email is already taken.");
        
        var existingUserByUsername = await _userManager.FindByNameAsync(userDto.UserName);
        if (existingUserByUsername != null)
            throw new ValidationException("Username is already taken.");

        var user = _mapper.Map<ApplicationUser>(userDto);
        user.EmailConfirmed = false;

        var result = await _userManager.CreateAsync(user, userDto.Password);
        if (!result.Succeeded)
            throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));

        await _userManager.AddToRoleAsync(user, userDto.Role.ToString());

        if (userDto.Role == Role.Client)
        {
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var activationLink = GenerateActivationLink(user.Email!, token);
            await SendActivationEmailAsync(user.Email!, activationLink);
        }
    }

    

    private string GenerateActivationLink(string email, string token)
    {
        var request = _httpContextAccessor.HttpContext?.Request;
        if (request == null)
            throw new InvalidOperationException("HttpContext is not available.");

        var baseUrl = $"{request.Scheme}://{request.Host}";
        return $"{baseUrl}/Account/activate?email={email}&token={_htmlEncoder.Encode(token)}";
    }
    private async Task SendActivationEmailAsync(string email, string activationLink)
    {
        string subject = "Activa tu cuenta";
        string body = $"<h1>{subject}</h1><p>Click <a href='{activationLink}'>here</a> to activate your account.</p>";
        await _emailService.SendEmailAsync(email, subject, body);
    }
    
    public async Task<LoginResult> LoginUserAsync(UserLoginDTO userDto)
    {
        var user = await _userManager.FindByEmailAsync(userDto.Username)
                   ?? await _userManager.FindByNameAsync(userDto.Username);

        if (user == null || !user.EmailConfirmed)
            return new LoginResult { IsSuccess = false }; // Retorno directo si hay errores.

        var result = await _signInManager.PasswordSignInAsync(user.UserName!, userDto.Password, isPersistent: false, lockoutOnFailure: false);
        if (!result.Succeeded)
            return new LoginResult { IsSuccess = false };

        var roles = await _userManager.GetRolesAsync(user);

        return new LoginResult
        {
            IsDeveloper = roles.Contains("Developer"),
            IsSuccess = true,
            IsAdmin = roles.Contains("Admin")
        };
    }


    
    
    public async Task<bool> ActivateUserAsync(string email, string token)
    {

        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
            return false;
        user.EmailConfirmed = true;
        var result = await _userManager.UpdateAsync(user);

        return result.Succeeded;
    }
    
    
    public async Task LogoutAsync()
    {
        await _signInManager.SignOutAsync();
    }

    private string GenerateJwtToken(ApplicationUser user)
    {
        var jwtSettings = _configuration.GetSection("JWTSettings").Get<JWTSettings>();

        var claims = new List<Claim>
    {
        new Claim(JwtRegisteredClaimNames.Sub, user.Id),
        new Claim(JwtRegisteredClaimNames.Email, user.Email!),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
    };

        var roles = _userManager.GetRolesAsync(user).Result;
        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings!.Key!));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(jwtSettings.DurationInMinutes),
            Issuer = jwtSettings.Issuer,
            Audience = jwtSettings.Audience,
            SigningCredentials = credentials
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }

    public async Task<LoginResponseDto> LoginAsync(LoginRequestDto request)
    {
        var user = await _userManager.FindByNameAsync(request.UserName);

        if (user == null || !await _userManager.CheckPasswordAsync(user, request.Password))
        {
            return new LoginResponseDto
            {
                Success = false,
                Errors = new List<string> { "Invalid login attempt." }
            };
        }

        var roles = await _userManager.GetRolesAsync(user);

        var allowedRoles = new List<string> { "Admin", "Developer" };

        if (!roles.Any(role => allowedRoles.Contains(role)))
        {
            return new LoginResponseDto
            {
                Success = false,
                Errors = new List<string> { "You do not have access to use this API." }
            };
        }

        return new LoginResponseDto
        {
            Success = true,
            UserId = user.Id,
            UserName = user.UserName!,
            Email = user.Email!,
            Role = roles.ToList(),
            Token = GenerateJwtToken(user)
        };
    }

    public async Task<RegisterResponseDto> RegisterAsync(RegisterRequestDto request, string role)
    {
        if (request.Password != request.ConfirmPassword)
        {
            return new RegisterResponseDto
            {
                Success = false,
                Errors = new List<string> { "Passwords do not match." }
            };
        }

        var user = new ApplicationUser
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            UserName = request.UserName,
            Email = request.Email,
            EmailConfirmed = true
        };

        var result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            return new RegisterResponseDto
            {
                Success = false,
                Errors = result.Errors.Select(e => e.Description).ToList()
            };
        }

        await _userManager.AddToRoleAsync(user, role);

        return new RegisterResponseDto
        {
            Success = true,
            UserId = user.Id,
            UserName = user.UserName,
            Email = user.Email
        };
    }
    
    public async Task<string> UpdateUserAsync(string userId, UserUpdateDTO userDto)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
            return "User not found.";
        
        user.FirstName = userDto.FirstName;
        user.LastName = userDto.LastName;
        user.PhoneNumber = userDto.PhoneNumber;
        
        if (userDto.Photo != null)
        {
            var photoPath = await FileHelper.SaveImageAsync(userDto.Photo!);
            user.ImagenPath = photoPath;
        }
        
        if (!string.IsNullOrEmpty(userDto.Password) && userDto.Password == userDto.ConfirmPassword)
        {
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var passwordResult = await _userManager.ResetPasswordAsync(user, token, userDto.Password);

            if (!passwordResult.Succeeded)
                return string.Join(", ", passwordResult.Errors.Select(e => e.Description));
        }
        
        var result = await _userManager.UpdateAsync(user);

        return result.Succeeded ? "successfully" : string.Join(", ", result.Errors.Select(e => e.Description));
    }
}