using AutoMapper;
using RealEstateApp.Application.Dtos.Account;
using RealEstateApp.Application.Interfaces.Repositories.Users;
using RealEstateApp.Application.Interfaces.Services.Users;
using RealEstateApp.Application.ViewModels.Users;

namespace RealEstateApp.Application.Services.Users;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public UserService(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<UserUpdateDTO> GetUserByIdDto(string id)
    {
        return await _userRepository.GetUserByIdDtoAsync(id);
    }

    public async Task<IList<AgentViewModel>> GetUsersByRoleAsync(string roleName)
    {
        return _mapper.Map<IList<AgentViewModel>>(await _userRepository.GetUsersByRoleAsync(roleName));
    }

    public async Task<IEnumerable<AgentViewModel>> GetAgentsByNameAsync(string name)
    {
        return _mapper.Map<IEnumerable<AgentViewModel>>(await _userRepository.GetAgentsByNameAsync(name));
    }
}