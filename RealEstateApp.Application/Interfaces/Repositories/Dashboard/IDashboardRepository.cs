namespace RealEstateApp.Application.Interfaces.Repositories.DashBoard;

public interface IDashboardRepository
{
    Task<int> GetSoldPropertiesCountAsync();
    Task<int> GetAvailablePropertyCountAsync();
    Task<int> GetActiveUsersByRoleAsync(string role);
    Task<int> GetInactiveUsersByRoleAsync(string role);
}