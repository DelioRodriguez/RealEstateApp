﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RealEstateApp.Application.Dtos.Account;
using RealEstateApp.Application.Dtos.Users;
using RealEstateApp.Application.Interfaces.Repositories.Users;
using RealEstateApp.Domain.Enums;
using RealEstateApp.Infrastructure.Identity.Entities;

namespace RealEstateApp.Infrastructure.Persistance.Repositories.Users;

public class UserRepository : IUserRepository
{
    private readonly UserManager<ApplicationUser> _userManager;

    public UserRepository(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<UserInfo> GetUserByIdAsync(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null) return null!;

        return new UserInfo()
        {
            AgentName = user.FirstName + " " + user.LastName,
            AgentPhone = user.PhoneNumber,
            AgentEmail = user.Email,
            AgentImageUrl = user.ImagenPath,
            AgentId = user.Id,
            Username = user.UserName
        };
    }

    public async Task<UserUpdateDTO> GetUserByIdDtoAsync(string id)
    {
        var user = await _userManager.FindByIdAsync(id);

        return new UserUpdateDTO()
        {
            ImagenPath = user!.ImagenPath,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            PhoneNumber = user.PhoneNumber,
            UserName = user.UserName!,
        };
    }

    public async Task<IList<UserInfo>> GetUsersByRoleAsync(string roleName)
    {
        if (string.IsNullOrEmpty(roleName))
        {
            throw new ArgumentNullException(nameof(roleName));
        }
        
        var users = await _userManager.GetUsersInRoleAsync(roleName);
        
        return users.Select(user => new UserInfo()
        {
            AgentId = user.Id,
            AgentImageUrl = user.ImagenPath,
            AgentEmail = user.Email,
            AgentName = user.FirstName + " " + user.LastName,
            AgentPhone = user.PhoneNumber
        }).ToList();
    }

    public async Task<IEnumerable<UserInfo>> GetAgentsByNameAsync(string name)
    {
        var agents = await _userManager.GetUsersInRoleAsync(Role.Agent.ToString());
        
        
        var filteredAgents = agents.Where(a => 
                a.UserName != null && ((a.UserName.Contains(name, StringComparison.OrdinalIgnoreCase)) ||
                                       (a.FirstName.Contains(name, StringComparison.OrdinalIgnoreCase)) ||
                                       (a.LastName.Contains(name, StringComparison.OrdinalIgnoreCase)) ||
                                       (string.Concat(a.FirstName, " ", a.LastName).Contains(name, StringComparison.OrdinalIgnoreCase)))
        );

        return filteredAgents.Select(a => new UserInfo()
        {
            AgentId = a.Id,
            AgentImageUrl = a.ImagenPath,
            AgentEmail = a.Email,
            AgentName = a.FirstName + " " + a.LastName,
            AgentPhone = a.PhoneNumber
        }).ToList();
    }

    public async Task<bool> UpdateEmailConfirmedStatusAsync(string agentId, bool status)
    {
        var user = await _userManager.FindByIdAsync(agentId);
        if (user == null)
        {
            return false;
        }

        user.EmailConfirmed = status;

        var result = await _userManager.UpdateAsync(user);

        return result.Succeeded;
    }
    
    public async Task<IEnumerable<UserInfo>> GetUsersByIdsAsync(IEnumerable<string> ids)
    {
        var users = await _userManager.Users.Where(u => ids.Contains(u.Id)).ToListAsync();
        
        return users.Select(user => new UserInfo
        {
            AgentId = user.Id,
            AgentImageUrl = user.ImagenPath,
            AgentEmail = user.Email,
            AgentName = user.FirstName + " " + user.LastName,
            AgentPhone = user.PhoneNumber,
            Username = user.UserName
        }).ToList();
    }


}