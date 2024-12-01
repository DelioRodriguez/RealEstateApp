using RealEstateApp.Application.Dtos.Account;
using RealEstateApp.Application.ViewModels.Users;

namespace RealEstateApp.Application.Interfaces.Services.Users;

public interface IUserService
{
    Task<AgentViewModel> GetUserByIdAsync(string id);
    Task<UserUpdateDTO> GetUserByIdDto(string id);
    Task<IList<AgentViewModel>> GetUsersByRoleAsync(string roleName);
    Task<IEnumerable<AgentViewModel>> GetAgentsByNameAsync(string name);
}