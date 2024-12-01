using RealEstateApp.Application.Dtos.Account;
using RealEstateApp.Application.Dtos.ApiAccount;
using RealEstateApp.Application.Dtos.Login;

namespace RealEstateApp.Application.Interfaces.Services.Account;

public interface IAccountService
{
    Task RegisterUserAsync(UserRegisterDTO userDto);
    Task<LoginResult> LoginUserAsync(UserLoginDTO userDto);
    Task<bool> ActivateUserAsync(string email, string token);
    Task LogoutAsync();

    #region Api
    Task<LoginResponseDto> LoginAsync(LoginRequestDto request);
    Task<RegisterResponseDto> RegisterAsync(RegisterRequestDto request, string role);
    #endregion
}