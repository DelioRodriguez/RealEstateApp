using RealEstateApp.Application.Dtos.Agents;

namespace RealEstateApp.Application.Interfaces.Repositories.Agent;

public interface IAgentRepository
{
    Task<List<AgentViewDto>> GetAgentsAsync();
    Task<bool> ToggleAgentActivationAsync(string agentId);
    Task DeleteAgentAsync(string agentId);
}