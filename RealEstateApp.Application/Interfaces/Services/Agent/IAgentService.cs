using RealEstateApp.Application.Dtos.Agents;

namespace RealEstateApp.Application.Interfaces.Services.Agent;

public interface IAgentService
{
    Task<List<AgentViewDto>> GetAgentsAsync();
    Task<bool> ToggleAgentActivationAsync(string agentId);
    Task DeleteAgentAsync(string agentId);
}
