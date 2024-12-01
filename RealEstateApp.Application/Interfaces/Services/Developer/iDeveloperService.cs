using RealEstateApp.Application.Dtos.Developer;

namespace RealEstateApp.Application.Interfaces.Services.Developer;

public interface iDeveloperService
{
    Task<IEnumerable<DeveloperDto>> GetAllDevelopersAsync();
    Task<DeveloperDto> GetDeveloperByIdAsync(string id);
    Task<bool> CreateDeveloperAsync(DeveloperDto developerDto);
    Task<bool> UpdateDeveloperAsync(DeveloperDto developerDto);
    Task<bool> ToggleDeveloperStatusAsync(string id, bool isActive);
}