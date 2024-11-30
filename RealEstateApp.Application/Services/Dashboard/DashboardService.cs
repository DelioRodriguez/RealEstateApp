using RealEstateApp.Application.Dtos.Dashboard;
using RealEstateApp.Application.Interfaces.Repositories.DashBoard;
using RealEstateApp.Application.Interfaces.Services.Dashboard;
using RealEstateApp.Domain.Enums;

namespace RealEstateApp.Application.Services.Dashboard;

public class DashboardService : IDashboardService
{
    private readonly IDashboardRepository _dashboardRepository;

    public DashboardService(IDashboardRepository dashboardRepository)
    {
        _dashboardRepository = dashboardRepository;
    }
    
    public async Task<DashboardDTO> GetDashboardDataAsync()
    {
        var dashboard = new DashboardDTO
        {
            SoldProperties = await _dashboardRepository.GetSoldPropertiesCountAsync(),
            AvailableProperties = await _dashboardRepository.GetAvailablePropertyCountAsync(),
            ActiveAgents = await _dashboardRepository.GetActiveUsersByRoleAsync(Role.Agent.ToString()),
            InactiveAgents = await _dashboardRepository.GetInactiveUsersByRoleAsync(Role.Agent.ToString()),
            ActiveClients = await _dashboardRepository.GetActiveUsersByRoleAsync(Role.Client.ToString()),
            InactiveClients = await _dashboardRepository.GetInactiveUsersByRoleAsync(Role.Client.ToString()),
            ActiveDevelopers = await _dashboardRepository.GetActiveUsersByRoleAsync(Role.Developer.ToString()),
            InactiveDevelopers = await _dashboardRepository.GetInactiveUsersByRoleAsync(Role.Developer.ToString())
        };
        return dashboard;
    }
}