using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using RealEstateApp.Application.Interfaces.Services.Generic;
using RealEstateApp.Application.Mapping;
using RealEstateApp.Application.Services.Generic;

namespace RealEstateApp.Application
{
    public static class ServiceRegistration
    {
        public static void AddApplicationService(this IServiceCollection services)
        {
            services.AddScoped(typeof(IService<>), typeof(Service<>));
            
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddAutoMapper(typeof(GeneralProfile));
        }
    }
}
