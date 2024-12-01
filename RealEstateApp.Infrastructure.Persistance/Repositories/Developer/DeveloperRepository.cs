using Microsoft.AspNetCore.Identity;
using RealEstateApp.Application.Dtos.Developer;
using RealEstateApp.Application.Interfaces.Repositories.Developer;
using RealEstateApp.Domain.Enums;
using RealEstateApp.Infrastructure.Identity.Entities;

namespace RealEstateApp.Infrastructure.Persistance.Repositories.Developer;

public class DeveloperRepository : IDeveloperRepository
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public DeveloperRepository(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }
    
    public async Task<IEnumerable<DeveloperDto>> GetAllDevelopersAsync()
    {
        var developers = await _userManager.GetUsersInRoleAsync(Role.Developer.ToString());
        return developers.Select(user => new DeveloperDto
        {
            Id = user.Id,
            UserName = user.UserName,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            IsActive = user.EmailConfirmed,
        });
    }

    public async Task<DeveloperDto> GetDeveloperByIdAsync(string id)
    {
      var user  = await _userManager.FindByIdAsync(id);
      if(user == null) return null;
      
      var roles = await _userManager.GetRolesAsync(user);
      if (!roles.Contains(Role.Developer.ToString())) return null;

      return new DeveloperDto
      {
          Id = user.Id,
          UserName = user.UserName,
          FirstName = user.FirstName,
          Email = user.Email,
          LastName = user.LastName,
          IsActive = user.EmailConfirmed,
      };
    }

    public async Task<bool> CreateDeveloperAsync(DeveloperDto developerDto)
    {
        var developer = new ApplicationUser
        {
            UserName = developerDto.UserName,
            Email = developerDto.Email,
            FirstName = developerDto.FirstName,
            LastName = developerDto.LastName,
            EmailConfirmed = true
        };
        
        var createResult = await _userManager.CreateAsync(developer, developerDto.Password);
        if(!createResult.Succeeded) return false;
        
        if (!await _roleManager.RoleExistsAsync("Developer"))
        {
            await _roleManager.CreateAsync(new IdentityRole("Developer"));
        }

        var addToRoleResult = await _userManager.AddToRoleAsync(developer, "Developer");
        return addToRoleResult.Succeeded;
    }

    public async Task<bool> UpdateDeveloperAsync(DeveloperDto developerDto)
    {
        var developer = await _userManager.FindByIdAsync(developerDto.Id);
        if (developer == null) return false;

        developer.FirstName = developerDto.FirstName;
        developer.LastName = developerDto.LastName;
        developer.Email = developerDto.Email;
        developer.UserName = developerDto.UserName;

        if (!string.IsNullOrWhiteSpace(developerDto.Password) && 
            !string.IsNullOrWhiteSpace(developerDto.ConfirmPassword))
        {
            if (developerDto.Password != developerDto.ConfirmPassword)
                throw new Exception("La contraseña y la confirmación no coinciden.");

            var removePasswordResult = await _userManager.RemovePasswordAsync(developer);
            if (!removePasswordResult.Succeeded)
                throw new Exception("Error al eliminar la contraseña existente.");

            var addPasswordResult = await _userManager.AddPasswordAsync(developer, developerDto.Password);
            if (!addPasswordResult.Succeeded)
                throw new Exception("Error al establecer la nueva contraseña.");
        }

        var updateResult = await _userManager.UpdateAsync(developer);
        return updateResult.Succeeded;
    }

    public async Task<bool> ToggleDeveloperStatusAsync(string id, bool isActive)
    {
        var developer = await _userManager.FindByIdAsync(id);
        if (developer == null) return false;

        developer.EmailConfirmed = isActive;
        var updateResult = await _userManager.UpdateAsync(developer);
        return updateResult.Succeeded;
    }
}