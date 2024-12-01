using RealEstateApp.Application.Dtos.Admin;

namespace RealEstateApp.Application.Interfaces.Repositories.Admin;

public interface IAdminRepository
{Task<IEnumerable<AdminDto>> GetAllAdminsAsync();
    Task<AdminDto> GetAdminByIdAsync(string id);
    Task<bool> CreateAdminAsync(AdminDto adminDto);
    Task<bool> UpdateAdminAsync(AdminDto adminDto);
    Task<bool> ToggleAdminStatusAsync(string id, bool isActive);
    
}