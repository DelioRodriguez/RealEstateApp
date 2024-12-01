using RealEstateApp.Application.Dtos.Account;
using RealEstateApp.Application.Dtos.Users;

namespace RealEstateApp.Application.Interfaces.Repositories.Users;

public interface IUserRepository
{
    Task<UserInfo> GetUserByIdAsync(string id);
    Task<UserUpdateDTO> GetUserByIdDtoAsync(string id);
    Task<IList<UserInfo>> GetUsersByRoleAsync(string roleName);
    Task<IEnumerable<UserInfo>> GetAgentsByNameAsync(string name);



    #region Api
    Task<bool> UpdateEmailConfirmedStatusAsync(string agentId, bool status);
    #endregion
}