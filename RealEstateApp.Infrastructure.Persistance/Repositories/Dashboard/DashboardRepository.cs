using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RealEstateApp.Application.Interfaces.Repositories.DashBoard;
using RealEstateApp.Infrastructure.Persistance.Context;

namespace RealEstateApp.Infrastructure.Persistance.Repositories.Dashboard;

public class DashboardRepository : IDashboardRepository
{
    private readonly  UserManager<IdentityUser> _userManager;
    private readonly AppDbContext _context;

    public DashboardRepository(UserManager<IdentityUser> userManager, AppDbContext context)
    {
        _userManager = userManager;
        _context = context;
    } 
    public async Task<int> GetSoldPropertiesCountAsync()
    {
        return await _context.Properties.CountAsync(p => !p.IsAvailable);
    }

    public async Task<int> GetAvailablePropertyCountAsync()
    {
        return await _context.Properties.CountAsync(p => p.IsAvailable);
    }

    public async Task<int> GetActiveUsersByRoleAsync(string role)
    {
        var usersInRole = await _userManager.GetUsersInRoleAsync(role);
        return usersInRole.Count(u => u.EmailConfirmed);
    }

    public async Task<int> GetInactiveUsersByRoleAsync(string role)
    {
        var usersInRole = await _userManager.GetUsersInRoleAsync(role);
        return usersInRole.Count(u => !u.EmailConfirmed);
    }
}