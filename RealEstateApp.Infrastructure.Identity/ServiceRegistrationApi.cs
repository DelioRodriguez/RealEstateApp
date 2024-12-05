using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using RealEstateApp.Application.Interfaces.Services.Account;
using RealEstateApp.Application.Mapping;
using RealEstateApp.Application.Settings;
using RealEstateApp.Infrastructure.Identity.Context;
using RealEstateApp.Infrastructure.Identity.Entities;
using RealEstateApp.Infrastructure.Identity.Seeds;
using RealEstateApp.Infrastructure.Identity.Services;
using RealEstateApp.Infrastructure.Shared.IService;
using RealEstateApp.Infrastructure.Shared.Service;

namespace RealEstateApp.Infrastructure.Identity;

public static class ServiceRegistrationAPI
{
    public static void AddIdentityInfrastructureApi(this IServiceCollection services, IConfiguration configuration)
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

     

        services.Configure<JWTSettings>(configuration.GetSection("JWTSettings"));

        var jwtSettings = configuration.GetSection("JWTSettings").Get<JWTSettings>();
        var key = Encoding.UTF8.GetBytes(jwtSettings.Key);

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = false;
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings.Issuer,
                ValidAudience = jwtSettings.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(key)
            };
        });
    }

    public static void AddIdentityServiceApi(this IServiceCollection services)
    {
        services.AddScoped<IEmailService, EmailService>(); 
        services.AddScoped<IAccountService, AccountService>();
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddAutoMapper(typeof(GeneralProfile));
    }

    public static async Task RunIdentitySeedsApi(this IServiceProvider serviceProvider)
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