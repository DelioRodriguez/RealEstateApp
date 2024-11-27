using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RealEstateApp.Application.Interfaces.Repositories.Api;
using RealEstateApp.Application.Interfaces.Repositories.Generic;
using RealEstateApp.Application.Interfaces.Repositories.Properties;
using RealEstateApp.Application.Interfaces.Repositories.Users;
using RealEstateApp.Infrastructure.Persistance.Context;
using RealEstateApp.Infrastructure.Persistance.Repositories;
using RealEstateApp.Infrastructure.Persistance.Repositories.Api;
using RealEstateApp.Infrastructure.Persistance.Repositories.Properties;
using RealEstateApp.Infrastructure.Persistance.Repositories.Users;

namespace RealEstateApp.Infrastructure.Persistance
{
    public static class ServiceRegistration
    {
        public static void AddContextInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IPropertyRepository, PropertyRepository>();
            services.AddScoped<IUserRepository, UserRepository>();

            #region Api

            services.AddScoped<IPropertiesApiRepository, PropertiesApiRepository>();
            services.AddScoped<IAgentApiRepository, AgentApiRepository>();
            services.AddScoped<IPropertyTypesApiRepository, PropertyTypesApiRepository>();
            services.AddScoped<ISaleTypeApiRepository, SaleTypeApiRepository>();
            services.AddScoped<IImprovementsApiRepository, ImprovementsApiRepository>();

            #endregion

            if (configuration.GetValue<bool>("UseInMemoryDatabase"))
            {
                services.AddDbContext<AppDbContext>(options =>
                {
                    options.UseInMemoryDatabase("ContextDb");
                });
            }
            else
            {
                services.AddDbContext<AppDbContext>(options =>
                {
                    options.EnableSensitiveDataLogging();
                    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                        mbox => mbox.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName));
                });
            }
        }
    }
}