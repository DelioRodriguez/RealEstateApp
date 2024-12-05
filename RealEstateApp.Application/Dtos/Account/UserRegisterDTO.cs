using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using RealEstateApp.Domain.Enums;

namespace RealEstateApp.Application.Dtos.Account;

public class UserRegisterDTO
{
    [Required(ErrorMessage = "Nombre es obligatorio")]
    public string FirstName { get; set; }
    [Required(ErrorMessage = "Apellido es obligatorio")]
    public string LastName { get; set; }
    [Required(ErrorMessage = "El teléfono es obligatorio.")]
    [Phone(ErrorMessage = "Debe ser un número de teléfono válido.")]
    public string? PhoneNumber { get; set; }
    public string? ImagenPath { get; set; }
    public IFormFile Photo { get; set; } 
    [Required(ErrorMessage = "Nombre de usuario es obligatorio")]
    public string UserName { get; set; }
    [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
    [EmailAddress(ErrorMessage = "Debe ser un correo electrónico válido.")]
    public string? Email { get; set; }
    [Required(ErrorMessage = "Contraseña es obligatorio")]
    [MinLength(6, ErrorMessage = "La contraseña debe tener al menos 8 caracteres y 1 especial")]
    public string Password { get; set; }
    [Required(ErrorMessage = "Debe confirmar su contraseña.")]
    [Compare("Password", ErrorMessage = "Las contraseñas no coinciden.")]
    public string ConfirmPassword { get; set; }
    public Role Role { get; set; }
}