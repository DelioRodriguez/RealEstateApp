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

        // Verificar cu�ntos usuarios son agentes
        Console.WriteLine($"N�mero de agentes encontrados: {users.Count}");

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
                IsActive = user.EmailConfirmed  // No filtrar aqu�, simplemente mostrar
            });

            // Verificar el estado de email confirmado para cada usuario
            Console.WriteLine($"Agente {user.Id} tiene email confirmado: {user.EmailConfirmed}");
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
        // Encuentra al usuario (agente)
        var user = await _userManager.FindByIdAsync(agentId);
        if (user == null)
        {
            throw new Exception("Agente no encontrado");
        }

        // Obtén las propiedades asociadas con el agente
        var properties = await _context.Properties.Where(x => x.AgentId == user.Id).ToListAsync();

        // Si el agente tiene propiedades asociadas, eliminarlas
        if (properties.Any())
        {
            _context.Properties.RemoveRange(properties);

            // Guardar los cambios en la base de datos después de eliminar las propiedades
            await _context.SaveChangesAsync();
        }

        // Eliminar al usuario (agente)
        var result = await _userManager.DeleteAsync(user);

        // Si la eliminación del agente falla, lanzar una excepción
        if (!result.Succeeded)
        {
            throw new Exception("No se pudo eliminar el agente");
        }

        // Guardar los cambios finales en la base de datos
        await _context.SaveChangesAsync();
    }


}