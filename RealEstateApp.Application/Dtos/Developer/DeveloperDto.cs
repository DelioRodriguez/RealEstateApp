namespace RealEstateApp.Application.Dtos.Developer;

public class DeveloperDto
{
    public string? Id { get; set; }
    public string? UserName { get; set; }
    public string? Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? Password { get; set; }
    public string? ConfirmPassword { get; set; }
    public bool IsActive { get; set; } 
}