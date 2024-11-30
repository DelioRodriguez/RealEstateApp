using RealEstateApp.Application.Dtos.Dashboard;

namespace RealEstateApp.Application.Interfaces.Services.Dashboard;

public interface IDashboardService
{
    Task<DashboardDTO> GetDashboardDataAsync();
}