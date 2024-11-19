using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RealEstateApp.Infrastructure.Identity.Context;
using RealEstateApp.Infrastructure.Identity.Entities;
using RealEstateApp.Infrastructure.Identity.Seeds;

namespace RealEstateApp.Infrastructure.Identity;

public static class ServiceRegistration
{
    public static void AddIdentityInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {


        if (configuration.GetValue<bool>("UseInMemoryDatabase"))
        {
            services.AddDbContext<IdentityContext>(options =>
            {
                options.UseInMemoryDatabase("IdentityDb");
            });
        }
        else
        {
            services.AddDbContext<IdentityContext>(options =>
            {
                options.EnableSensitiveDataLogging();
                options.UseSqlServer(configuration.GetConnectionString("IdentityConnection"),
                    mbox => mbox.MigrationsAssembly(typeof(IdentityContext).Assembly.FullName));
            });
        }

        services.AddIdentityCore<ApplicationUser>()
            .AddRoles<IdentityRole>()
            .AddSignInManager()
            .AddEntityFrameworkStores<IdentityContext>()
            .AddTokenProvider<DataProtectorTokenProvider<ApplicationUser>>(TokenOptions.DefaultProvider);

        services.Configure<DataProtectionTokenProviderOptions>(opt =>
        {
            opt.TokenLifespan = TimeSpan.FromSeconds(300);
        });

        services.AddAuthentication(opt =>
        {
            opt.DefaultScheme = IdentityConstants.ApplicationScheme;
            opt.DefaultChallengeScheme = IdentityConstants.ApplicationScheme;
            opt.DefaultSignInScheme = IdentityConstants.ApplicationScheme;
        }).AddCookie(IdentityConstants.ApplicationScheme, opt =>
        {
            opt.ExpireTimeSpan = TimeSpan.FromHours(24);
            opt.LoginPath = "/User";
            opt.AccessDeniedPath = "/User/AccessDenied";
        });
    }
    
    public static async Task RunIdentitySeeds(this IServiceProvider serviceProvider)
    {
        using (var scope = serviceProvider.CreateScope())
        {
            var services = scope.ServiceProvider;

            try
            {
                var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
                var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

                await IdentitySeeder.SeedAsync(userManager, roleManager);
            }
            catch (Exception ex)
            {
                // ignored
            }
        }
    }
}