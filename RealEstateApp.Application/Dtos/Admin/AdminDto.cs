namespace RealEstateApp.Application.Dtos.Admin;

public class AdminDto
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