using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using RealEstateApp.Application.Interfaces.Services.Admin;
using RealEstateApp.Application.Interfaces.Services.Agent;
using RealEstateApp.Application.Interfaces.Services.Api;
using RealEstateApp.Application.Interfaces.Services.Dashboard;
using RealEstateApp.Application.Interfaces.Services.Developer;
using RealEstateApp.Application.Interfaces.Services.Favory;
using RealEstateApp.Application.Interfaces.Services.Generic;
using RealEstateApp.Application.Interfaces.Services.Improvements;
using RealEstateApp.Application.Interfaces.Services.Properties;
using RealEstateApp.Application.Interfaces.Services.PropertiesType;
using RealEstateApp.Application.Interfaces.Services.Users;
using RealEstateApp.Application.Mapping;
using RealEstateApp.Application.Services.Admin;
using RealEstateApp.Application.Services.Api;
using RealEstateApp.Application.Services.Dashboard;
using RealEstateApp.Application.Services.Favory;
using RealEstateApp.Application.Services.Generic;
using RealEstateApp.Application.Services.Improvements;
using RealEstateApp.Application.Services.Properties;
using RealEstateApp.Application.Services.PropertiesType;
using RealEstateApp.Application.Services.Users;
using RealEstateApp.Application.Services.Agent;
using RealEstateApp.Application.Services.Developer;

namespace RealEstateApp.Application
{
    public static class ServiceRegistration
    {
        public static void AddApplicationService(this IServiceCollection services)
        {
            services.AddScoped(typeof(IService<>), typeof(Service<>));
            services.AddScoped<IPropertyService, PropertyService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IFavoriteService, FavoriteService>();
            services.AddScoped<IImprovementService, ImprovementService>();
            services.AddScoped<IDashboardService, DashboardService>();
            services.AddScoped<IPropertiesTypeServices, PropertiesTypeServices>();
            services.AddScoped<IAgentService, AgentService>();
            services.AddScoped<IAdminService, AdminService>();
            services.AddScoped<iDeveloperService,DeveloperService>();


            #region Api

            services.AddScoped<IPropertiesApiService, PropertiesApiService>();
            services.AddScoped<IAgentApiService, AgentApiService>();
            services.AddScoped<IPropertyTypesApiService, PropertyTypesApiService>();
            services.AddScoped<ISaleTypeApiService, SaleTypeApiService>();
            services.AddScoped<IImprovementsApiService, ImprovementsApiService>();

            #endregion

            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddAutoMapper(typeof(GeneralProfile));
        }
    }
}
