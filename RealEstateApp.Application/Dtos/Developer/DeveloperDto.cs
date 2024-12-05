namespace RealEstateApp.Application.Dtos.Developer;
using System.ComponentModel.DataAnnotations;

public class DeveloperDto
{
    public string? Id { get; set; }
    [Required(ErrorMessage = "El username es requerido")]
    public string? UserName { get; set; }
    [Required(ErrorMessage = "El email es requerido")]
    public string? Email { get; set; }
    [Required(ErrorMessage = "El Nombre es requerido")]
    public string FirstName { get; set; }
    [Required(ErrorMessage = "El Apellido es requerido")]
    public string LastName { get; set; }
    public string? Password { get; set; }
    [Compare("Password", ErrorMessage = "Las contraseñas no coinciden.")]
    public string? ConfirmPassword { get; set; }
    public bool IsActive { get; set; } 
}