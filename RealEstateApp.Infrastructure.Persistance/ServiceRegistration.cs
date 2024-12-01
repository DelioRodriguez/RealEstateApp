using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RealEstateApp.Application.Interfaces.Repositories.Api;
using RealEstateApp.Application.Interfaces.Repositories.DashBoard;
using RealEstateApp.Application.Interfaces.Repositories.Favory;
using RealEstateApp.Application.Interfaces.Repositories.Generic;
using RealEstateApp.Application.Interfaces.Repositories.Improvements;
using RealEstateApp.Application.Interfaces.Repositories.Properties;
using RealEstateApp.Application.Interfaces.Repositories.PropertiesType;
using RealEstateApp.Application.Interfaces.Repositories.SalesType;
using RealEstateApp.Application.Interfaces.Repositories.Users;
using RealEstateApp.Infrastructure.Persistance.Context;
using RealEstateApp.Infrastructure.Persistance.Repositories;
using RealEstateApp.Infrastructure.Persistance.Repositories.Api;
using RealEstateApp.Infrastructure.Persistance.Repositories.Dashboard;
using RealEstateApp.Infrastructure.Persistance.Repositories.Favory;
using RealEstateApp.Infrastructure.Persistance.Repositories.Improvements;
using RealEstateApp.Infrastructure.Persistance.Repositories.Properties;
using RealEstateApp.Infrastructure.Persistance.Repositories.PropertiesType;
using RealEstateApp.Infrastructure.Persistance.Repositories.SalesType;
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
            services.AddScoped<IFavoriteRepository, FavoriteRepository>();
            services.AddScoped<ISaleTypeRepository, SaleTypeRepository>();
            services.AddScoped<IPropertyTypeRepository, PropertyTypeRepository>();
            services.AddScoped<IImprovementRepository, ImprovementRepository>();
            services.AddScoped<IDashboardRepository, DashboardRepository>();


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