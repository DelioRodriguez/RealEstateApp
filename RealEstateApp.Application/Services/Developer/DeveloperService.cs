using RealEstateApp.Application.Dtos.Developer;
using RealEstateApp.Application.Interfaces.Repositories.Developer;
using RealEstateApp.Application.Interfaces.Services.Developer;

namespace RealEstateApp.Application.Services.Developer;

public class DeveloperService : iDeveloperService
{
    private readonly IDeveloperRepository _developerRepository;

    public DeveloperService(IDeveloperRepository developerRepository)
    {
        _developerRepository = developerRepository;
    }
    
    public async Task<IEnumerable<DeveloperDto>> GetAllDevelopersAsync()
    {
        return await _developerRepository.GetAllDevelopersAsync();
    }

    public async Task<DeveloperDto> GetDeveloperByIdAsync(string id)
    {
       return await _developerRepository.GetDeveloperByIdAsync(id); 
    }

    public async Task<bool> CreateDeveloperAsync(DeveloperDto developerDto)
    {
       return await _developerRepository.CreateDeveloperAsync(developerDto); 
    }

    public async Task<bool> UpdateDeveloperAsync(DeveloperDto developerDto)
    {
        return await  _developerRepository.UpdateDeveloperAsync(developerDto);
    }

    public async Task<bool> ToggleDeveloperStatusAsync(string id, bool isActive)
    {
        return await _developerRepository.ToggleDeveloperStatusAsync(id, isActive); 
    }
}