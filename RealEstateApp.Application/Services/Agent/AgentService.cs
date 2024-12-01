using RealEstateApp.Application.Dtos.Agents;
using RealEstateApp.Application.Interfaces.Repositories.Agent;
using RealEstateApp.Application.Interfaces.Services.Agent;

namespace RealEstateApp.Application.Services.Agent;

public class AgentService : IAgentService
{
    private readonly IAgentRepository _agentRepository;

    public AgentService(IAgentRepository agentRepository)
    {
        _agentRepository = agentRepository;
    }
    
    public async Task<List<AgentViewDto>> GetAgentsAsync()
    {
        return await _agentRepository.GetAgentsAsync(); 
    }

    public async Task<bool> ToggleAgentActivationAsync(string agentId)
    {
        return await _agentRepository.ToggleAgentActivationAsync(agentId);
    }

    public async Task DeleteAgentAsync(string agentId)
    {
       await _agentRepository.DeleteAgentAsync(agentId);
    }
}