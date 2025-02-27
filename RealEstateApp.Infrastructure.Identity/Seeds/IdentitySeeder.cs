﻿using Microsoft.AspNetCore.Identity;
using RealEstateApp.Domain.Enums;
using RealEstateApp.Infrastructure.Identity.Entities;

namespace RealEstateApp.Infrastructure.Identity.Seeds;

public class IdentitySeeder
{
    public static async Task SeedAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        var roles = new[] { Role.Admin.ToString(), Role.Client.ToString(), Role.Agent.ToString(), Role.Developer.ToString() };

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }
        
        var adminEmail = "admin@email.com";
        var adminUser = await userManager.FindByEmailAsync(adminEmail);

        if (adminUser == null)
        {
            adminUser = new ApplicationUser
            {
                ImagenPath = "https://www.shutterstock.com/image-vector/special-secret-agent-tuxedo-armed-600nw-2319552755.jpg",
                FirstName = "admin",
                LastName = "admin",
                UserName = "admin",
                Email = adminEmail,
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };
            
            await userManager.CreateAsync(adminUser, "Agent12345!");
            await userManager.AddToRoleAsync(adminUser, Role.Admin.ToString());
        }
        
        var agentEmail = "agent@email.com";
        var agentUser = await userManager.FindByEmailAsync(agentEmail);
        if (agentUser == null)
        {
            agentUser = new ApplicationUser
            {
                ImagenPath = "https://www.shutterstock.com/image-vector/special-secret-agent-tuxedo-armed-600nw-2319552755.jpg",
                FirstName = "Ivo",
                LastName = "Rodriguez",
                PhoneNumber = "8293630460",
                UserName = "agent",
                Email = agentEmail,
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };

            await userManager.CreateAsync(agentUser, "Agent12345!");
            await userManager.AddToRoleAsync(agentUser, Role.Agent.ToString());
        }
        
        var developerEmail = "Developer@email.com";
        var developerUser = await userManager.FindByEmailAsync(developerEmail);
        
        if (developerUser == null)
        {
            developerUser = new ApplicationUser
            {
                ImagenPath = "https://superlabs.co/assets/img/innerimg/frontend-developer.jpg",
                FirstName = "Developer",
                LastName = "BackEnd",
                PhoneNumber = "8290000000",
                UserName = "Developer",
                Email = developerEmail,
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };

            await userManager.CreateAsync(developerUser, "Developer123.@");
            await userManager.AddToRoleAsync(developerUser, Role.Developer.ToString());
        }
    }
}