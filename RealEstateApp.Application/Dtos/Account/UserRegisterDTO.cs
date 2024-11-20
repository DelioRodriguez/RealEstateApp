using Microsoft.AspNetCore.Http;
using RealEstateApp.Domain.Enums;

namespace RealEstateApp.Application.Dtos.Account;

public class UserRegisterDTO
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? PhoneNumber { get; set; }
    public string? ImagenPath { get; set; }
    public IFormFile Photo { get; set; } 
    public string UserName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string ConfirmPassword { get; set; }
    public Role Role { get; set; }
}