using Microsoft.AspNetCore.Identity;
using RealEstateApp.Application.Dtos.Admin;
using RealEstateApp.Application.Interfaces.Repositories.Admin;
using RealEstateApp.Domain.Enums;
using RealEstateApp.Infrastructure.Identity.Entities;

namespace RealEstateApp.Infrastructure.Persistance.Repositories.Admin;

public class AdminRepository : IAdminRepository
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public AdminRepository(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }
    
    
    public async Task<IEnumerable<AdminDto>> GetAllAdminsAsync()
    {
        var adminRole = await _roleManager.FindByNameAsync(Role.Admin.ToString());
        if (adminRole == null)
        {
            return new List<AdminDto>();
        }
        var admins =  await _userManager.GetUsersInRoleAsync(Role.Admin.ToString());

        return admins.Select(admin => new AdminDto
        {
            Id = admin.Id,
            UserName = admin.UserName,
            Email = admin.Email,
            FirstName = admin.FirstName,
            LastName = admin.LastName,
            IsActive = admin.EmailConfirmed
        });
    }

    public async Task<AdminDto> GetAdminByIdAsync(string id)
    {
      var user = await _userManager.FindByIdAsync(id);
      
      if (user == null || !(await _userManager.IsInRoleAsync(user, Role.Admin.ToString()))) return null;

      return new AdminDto
      {
          Id = user.Id,
          UserName = user.UserName,
          Email = user.Email,
          FirstName = user.FirstName,
          LastName = user.LastName,
          IsActive = user.EmailConfirmed
      };
    }

    public async Task<bool> CreateAdminAsync(AdminDto adminDto)
    {
        var admin = new ApplicationUser
        {
            UserName = adminDto.UserName,
            Email = adminDto.Email,
            FirstName = adminDto.FirstName,
            LastName = adminDto.LastName,
            EmailConfirmed = true
        };
        var resultPassword = await _userManager.CreateAsync(admin, adminDto.Password);
        if(!resultPassword.Succeeded) return false;
        
        var result = await _userManager.AddToRoleAsync(admin, Role.Admin.ToString());
        return result.Succeeded;
    }
    public async Task<bool> UpdateAdminAsync(AdminDto adminDto)
    {
     
        var admin = await _userManager.FindByIdAsync(adminDto.Id);
        if (admin == null)
            return false;

        admin.FirstName = adminDto.FirstName;
        admin.LastName = adminDto.LastName;
        admin.Email = adminDto.Email;
        admin.UserName = adminDto.UserName;

        if (!string.IsNullOrWhiteSpace(adminDto.Password) &&
            !string.IsNullOrWhiteSpace(adminDto.ConfirmPassword))
        {
            if (adminDto.Password != adminDto.ConfirmPassword)
            {
                throw new Exception("La contraseña y la confirmación no coinciden.");
            }

            var passwordChangeResult = await _userManager.RemovePasswordAsync(admin);
            if (!passwordChangeResult.Succeeded)
            {
                throw new Exception("Error al eliminar la contraseña existente.");
            }

            var addPasswordResult = await _userManager.AddPasswordAsync(admin, adminDto.Password);
            if (!addPasswordResult.Succeeded)
            {
                throw new Exception("Error al establecer la nueva contraseña.");
            }
        }

        var updateResult = await _userManager.UpdateAsync(admin);
        return updateResult.Succeeded;
    }


    public async Task<bool> ToggleAdminStatusAsync(string id, bool isActive)
    {
        var admin = await _userManager.FindByIdAsync(id);
        if(admin == null) return false;
        
        admin.EmailConfirmed = isActive;
        
        var result = await _userManager.UpdateAsync(admin);
        return result.Succeeded;
    }
}