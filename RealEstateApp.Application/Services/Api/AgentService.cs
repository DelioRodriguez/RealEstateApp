using AutoMapper;
using RealEstateApp.Application.Dtos.Agents;
using RealEstateApp.Application.Interfaces.Repositories.Api;
using RealEstateApp.Application.Interfaces.Repositories.Users;
using RealEstateApp.Application.Interfaces.Services.Api;
using RealEstateApp.Domain.Enums;

namespace RealEstateApp.Application.Services.Api
{
    public class AgentService : IAgentApiService
    {
        private readonly IUserRepository _userRepository;
        private readonly IAgentApiRepository _agentApiRepository;
        private readonly IMapper _mapper;

        public AgentService(IUserRepository userRepository, IAgentApiRepository agentApiRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _agentApiRepository = agentApiRepository;
            _mapper = mapper;
        }

        public async Task<List<AgentDto>> GetAllAgentsAsync()
        {
            var users = await _userRepository.GetUsersByRoleAsync(Role.Agent.ToString());
            var agentIds = users.Select(u => u.AgentId).ToList();

            var propertyCounts = await _agentApiRepository
                .GetPropertiesGroupedByAgentAsync(agentIds);

            var agents = users.Select(user => new AgentDto
            {
                AgentId = user.AgentId,
                FirstName = user.AgentName.Split(' ')[0],
                LastName = user.AgentName.Split(' ').Skip(1).FirstOrDefault() ?? "",
                Email = user.AgentEmail,
                Phone = user.AgentPhone,
                Properties = propertyCounts.ContainsKey(user.AgentId) ? propertyCounts[user.AgentId] : 0
            }).ToList();

            return agents;
        }

        public async Task<AgentDto?> GetAgentByIdAsync(string agentId)
        {
            var user = await _userRepository.GetUserByIdAsync(agentId);
            if (user == null) return null;

            var propertyCount = await _agentApiRepository.CountByAgentId(agentId);

            return new AgentDto
            {
                AgentId = user.AgentId,
                FirstName = user.AgentName.Split(' ')[0],
                LastName = user.AgentName.Split(' ').Skip(1).FirstOrDefault() ?? "",
                Email = user.AgentEmail,
                Phone = user.AgentPhone,
                Properties = propertyCount
            };
        }

        public async Task<List<AgentPropertiesDto>> GetAgentPropertiesAsync(string agentId)
        {
            var properties = await _agentApiRepository.GetPropertiesByAgentIdAsync(agentId);

            if (properties == null || !properties.Any())
                return new List<AgentPropertiesDto>();

            return _mapper.Map<List<AgentPropertiesDto>>(properties);
        }


        public async Task<bool> ChangeAgentStatusAsync(string agentId, bool status)
        {
            return await _userRepository.UpdateEmailConfirmedStatusAsync(agentId, status);
        }

    }
}
