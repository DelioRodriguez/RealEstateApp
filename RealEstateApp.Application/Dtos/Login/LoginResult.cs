namespace RealEstateApp.Application.Dtos.Login;

public class LoginResult
{
    public bool IsSuccess { get; set; } 
    public bool IsAdmin { get; set; }  
    
    public bool IsDeveloper { get; set; }
}