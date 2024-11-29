using RealEstateApp.Domain.Entities;

namespace RealEstateApp.Application.Interfaces.Repositories.Api
{
    public interface IAgentApiRepository
    {
        Task<int> CountByAgentId(string agentId);
        Task<List<Property>> GetPropertiesByAgentIdAsync(string agentId);
        Task<Dictionary<string, int>> GetPropertiesGroupedByAgentAsync(List<string> agentIds);
    }
}
