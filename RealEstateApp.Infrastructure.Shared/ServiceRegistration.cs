using Microsoft.Extensions.DependencyInjection;
using RealEstateApp.Infrastructure.Shared.IService;
using RealEstateApp.Infrastructure.Shared.Service;

namespace RealEstateApp.Infrastructure.Shared;

public static class ServiceRegistration
{
    public static void AddSharedService(this IServiceCollection services)
    {
        services.AddScoped<IEmailService, EmailService>();
    }
}