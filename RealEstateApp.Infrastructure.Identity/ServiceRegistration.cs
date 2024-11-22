using System.Reflection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RealEstateApp.Application.Interfaces.Services.Account;
using RealEstateApp.Application.Mapping;
using RealEstateApp.Infrastructure.Identity.Context;
using RealEstateApp.Infrastructure.Identity.Entities;
using RealEstateApp.Infrastructure.Identity.Seeds;
using RealEstateApp.Infrastructure.Identity.Services;

namespace RealEstateApp.Infrastructure.Identity;

public static class ServiceRegistration
{
    public static void AddIdentityInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
       
        services.AddSingleton(TimeProvider.System);

       
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

     
        services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
              
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
            })
            .AddEntityFrameworkStores<IdentityContext>()
            .AddSignInManager<SignInManager<ApplicationUser>>()
            .AddDefaultTokenProviders(); 


     
        services.Configure<DataProtectionTokenProviderOptions>(opt =>
        {
            opt.TokenLifespan = TimeSpan.FromSeconds(300);
        });

        services.ConfigureApplicationCookie(opt =>
        {
            opt.ExpireTimeSpan = TimeSpan.FromHours(24);
            opt.LoginPath = "/User";
            opt.AccessDeniedPath = "/User/AccessDenied";
        });
       
    }

    public static void AddIdentityService(this IServiceCollection services)
    {
        services.AddScoped<IAccountService, AccountService>();
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddAutoMapper(typeof(GeneralProfile));
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
                throw new Exception("An error occurred while seeding the database.", ex);
            }
        }
    }
}
