using RealEstateApp.Application.Dtos.Users;

namespace RealEstateApp.Application.Interfaces.Repositories.Users;

public interface IUserRepository
{
    Task<UserInfo> GetUserByIdAsync(string id);
    Task<IList<UserInfo>> GetUsersByRoleAsync(string roleName);
    Task<IEnumerable<UserInfo>> GetAgentsByNameAsync(string name);
}