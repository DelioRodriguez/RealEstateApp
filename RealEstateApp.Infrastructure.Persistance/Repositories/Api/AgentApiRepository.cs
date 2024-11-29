using Microsoft.EntityFrameworkCore;
using RealEstateApp.Application.Interfaces.Repositories.Api;
using RealEstateApp.Domain.Entities;
using RealEstateApp.Infrastructure.Persistance.Context;

namespace RealEstateApp.Infrastructure.Persistance.Repositories.Api
{
    public class AgentApiRepository : IAgentApiRepository
    {
        private readonly AppDbContext _context;

        public AgentApiRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<int> CountByAgentId(string agentId)
        {
            return await _context.Properties.CountAsync(p => p.AgentId == agentId);
        }

        public async Task<List<Property>> GetPropertiesByAgentIdAsync(string agentId)
        {
            return await _context.Properties
            .Where(p => p.AgentId == agentId)
            .Include(p => p.PropertyType)
            .Include(p => p.SaleType)
            .Include(p => p.Improvements)
            .ToListAsync();
        }

        public async Task<Dictionary<string, int>> GetPropertiesGroupedByAgentAsync(List<string> agentIds)
        {
            return await _context.Properties
                .Where(p => agentIds.Contains(p.AgentId))
                .GroupBy(p => p.AgentId)
                .ToDictionaryAsync(g => g.Key, g => g.Count());
        }

    }
}
