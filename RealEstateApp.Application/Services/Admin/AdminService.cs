using RealEstateApp.Application.Dtos.Admin;
using RealEstateApp.Application.Interfaces.Repositories.Admin;
using RealEstateApp.Application.Interfaces.Services.Admin;

namespace RealEstateApp.Application.Services.Admin;

public class AdminService : IAdminService
{
    private readonly IAdminRepository _adminRepository;

    public AdminService(IAdminRepository adminRepository)
    {
        _adminRepository = adminRepository;
    }
    
    public Task<IEnumerable<AdminDto>> GetAllAdminsAsync()
    {
        return _adminRepository.GetAllAdminsAsync();
    }

    public Task<AdminDto> GetAdminByIdAsync(string id)
    {
        return _adminRepository.GetAdminByIdAsync(id);
    }

    public Task<bool> CreateAdminAsync(AdminDto adminDto)
    {
        return _adminRepository.CreateAdminAsync(adminDto);
    }

    public Task<bool> UpdateAdminAsync(AdminDto adminDto)
    {
      return _adminRepository.UpdateAdminAsync(adminDto);
    }

    public Task<bool> ToggleAdminStatusAsync(string id, bool isActive)
    {
       return _adminRepository.ToggleAdminStatusAsync(id, isActive);
    }
}