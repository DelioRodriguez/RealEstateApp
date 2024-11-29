using System.IdentityModel.Tokens.Jwt;
using System.Net.Mail;
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
using RealEstateApp.Application.Interfaces.Services.Account;
using RealEstateApp.Application.Interfaces.Services.Users;
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
    
    
    public async Task<string> RegisterUserAsync(UserRegisterDTO userDto)
    {
        if (userDto.Password != userDto.ConfirmPassword)
            return "Passwords do not match.";

        var user = _mapper.Map<ApplicationUser>(userDto);
        user.EmailConfirmed = false;

        var result = await _userManager.CreateAsync(user, userDto.Password);
        if (!result.Succeeded)
            return string.Join(", ", result.Errors.Select(e => e.Description));

        await _userManager.AddToRoleAsync(user, userDto.Role.ToString());

        if (userDto.Role == Role.Client)
        {
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var activationLink = GenerateActivationLink(user.Email, token);

            await SendActivationEmailAsync(user.Email, activationLink);
        }

        return "Registration successful. Please login.";
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
    
    
    public async Task<string> LoginUserAsync(UserLoginDTO userDTO)
    {
        var user = await _userManager.FindByEmailAsync(userDTO.Username)
                   ?? await _userManager.FindByNameAsync(userDTO.Username);

        if (user == null)
            return "Invalid credentials.";

        if (!user.EmailConfirmed)
            return "Your email has not been confirmed. Please check your inbox.";

        var result = await _signInManager.PasswordSignInAsync(user.UserName, userDTO.Password, isPersistent: false, lockoutOnFailure: false);

        if (!result.Succeeded)
            return "Invalid credentials.";

        return "Login successful.";
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
        new Claim(JwtRegisteredClaimNames.Email, user.Email),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
    };

        var roles = _userManager.GetRolesAsync(user).Result;
        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key));
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
            UserName = user.UserName,
            Email = user.Email,
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
}