using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using RealEstateApp.Domain.Enums;

namespace RealEstateApp.Application.Dtos.Account;

public class UserUpdateDTO
{
    [Required]
    public string FirstName { get; set; }

    [Required]
    public string LastName { get; set; }

    public string? PhoneNumber { get; set; }

    public IFormFile? Photo { get; set; }
    public string? ImagenPath { get; set; }

    [Required]
    public string UserName { get; set; }

    [EmailAddress]
    public string? Email { get; set; }

    [MinLength(8)]
    public string? Password { get; set; }

    [Compare("Password")]
    public string? ConfirmPassword { get; set; }
}