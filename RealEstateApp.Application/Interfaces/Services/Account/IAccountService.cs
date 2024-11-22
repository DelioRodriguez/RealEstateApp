using RealEstateApp.Application.Dtos.Account;

namespace RealEstateApp.Application.Interfaces.Services.Account;

public interface IAccountService
{
    Task<string> RegisterUserAsync(UserRegisterDTO userDTO);
    Task<string> LoginUserAsync(UserLoginDTO userDTO);
    Task<bool> ActivateUserAsync(string email, string token);
    Task LogoutAsync();
}