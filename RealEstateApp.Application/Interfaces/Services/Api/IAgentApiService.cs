using RealEstateApp.Application.Dtos.Agents;

namespace RealEstateApp.Application.Interfaces.Services.Api
{
    public interface IAgentApiService
    {
        Task<List<AgentDto>> GetAllAgentsAsync();
        Task<AgentDto?> GetAgentByIdAsync(string agentId);
        Task<List<AgentPropertiesDto>> GetAgentPropertiesAsync(string agentId);
        Task<bool> ChangeAgentStatusAsync(string agentId, bool status);
    }
}
