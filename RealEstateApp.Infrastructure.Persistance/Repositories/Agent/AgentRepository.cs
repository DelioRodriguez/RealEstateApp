using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RealEstateApp.Application.Dtos.Agents;
using RealEstateApp.Application.Interfaces.Repositories.Agent;
using RealEstateApp.Domain.Enums;
using RealEstateApp.Infrastructure.Identity.Entities;
using RealEstateApp.Infrastructure.Persistance.Context;

namespace RealEstateApp.Infrastructure.Persistance.Repositories.Agent;

public class AgentRepository : IAgentRepository
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly AppDbContext _context;

    public AgentRepository(UserManager<ApplicationUser> userManager, AppDbContext context)
    {
        _userManager = userManager;
        _context = context;
    }

    public async Task<List<AgentViewDto>> GetAgentsAsync()
    {
        var users = await _userManager.GetUsersInRoleAsync(Role.Agent.ToString());
        var result = new List<AgentViewDto>();
        foreach (var user in users)
        {
            var propertiesCount = await _context.Properties.CountAsync(x => x.AgentId == user.Id);
            
            result.Add(new AgentViewDto
            {
                AgentId = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Phone = user.PhoneNumber,
                Properties = propertiesCount,
                IsActive = user.EmailConfirmed
            });
        }
        return result;
    }

    public async Task<bool> ToggleAgentActivationAsync(string agentId)
    {
        var user = await _userManager.FindByIdAsync(agentId);
        if (user == null) throw new Exception("Agente no encontrado");
        user.EmailConfirmed = !user.EmailConfirmed;
        
        var result = await _userManager.UpdateAsync(user);
        return result.Succeeded;
    }

    public async Task DeleteAgentAsync(string agentId)
    {
        var user = await _userManager.FindByIdAsync(agentId);
        if(user == null) throw new Exception("Agente no encontrado");
        
        var propeties = _context.Properties.Where(x => x.AgentId == user.Id);
        _context.Properties.RemoveRange(propeties);
        
        var result = await _userManager.DeleteAsync(user);

        if (!result.Succeeded)
        {
            throw new Exception("No se pudo eliminar el agente");
        }
        await _context.SaveChangesAsync();
    }
}