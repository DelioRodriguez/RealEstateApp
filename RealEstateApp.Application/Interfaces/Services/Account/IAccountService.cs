using RealEstateApp.Application.Dtos.Account;
using RealEstateApp.Application.Dtos.ApiAccount;

namespace RealEstateApp.Application.Interfaces.Services.Account;

public interface IAccountService
{
    Task<string> RegisterUserAsync(UserRegisterDTO userDTO);
    Task<string> LoginUserAsync(UserLoginDTO userDTO);
    Task<bool> ActivateUserAsync(string email, string token);

    #region Api
    Task<LoginResponseDto> LoginAsync(LoginRequestDto request);
    Task<RegisterResponseDto> RegisterAsync(RegisterRequestDto request, string role);
    #endregion
}