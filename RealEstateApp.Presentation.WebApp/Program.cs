using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RealEstateApp.Application;
using RealEstateApp.Application.Interfaces.Repositories.DashBoard;
using RealEstateApp.Application.Interfaces.Services.Dashboard;
using RealEstateApp.Application.Services.Dashboard;
using RealEstateApp.Application.Settings;
using RealEstateApp.Domain.Enums;
using RealEstateApp.Infrastructure.Identity;
using RealEstateApp.Infrastructure.Persistance;
using RealEstateApp.Infrastructure.Persistance.Repositories.Dashboard;
using RealEstateApp.Infrastructure.Shared;

var builder = WebApplication.CreateBuilder(args);

// Agregar los servicios de identidad e infraestructura
builder.Services.AddIdentityInfrastructure(builder.Configuration);
builder.Services.AddContextInfrastructure(builder.Configuration);
builder.Services.AddApplicationService();

// Configuración de correo SMTP desde appsettings.json
builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("SmtpSettings"));

// Servicios compartidos
builder.Services.AddSharedService();
builder.Services.AddIdentityService();


builder.Services.AddControllersWithViews();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login"; 
        options.AccessDeniedPath = "/Properties/index";  // Redirigir a la página deseada.
    });




builder.Services.AddAuthorization(options =>
{
   
    options.AddPolicy("AdminOnly", policy => policy.RequireRole(Role.Admin.ToString()));
    options.AddPolicy("ClientOnly", policy => policy.RequireRole(Role.Client.ToString()));
    options.AddPolicy("AgentOnly", policy => policy.RequireRole(Role.Agent.ToString()));
    
    
});

var app = builder.Build();

// Usar manejo de excepciones en entornos de producción
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// Ejecutar las semillas de la identidad (roles predeterminados, etc.)
await app.Services.RunIdentitySeeds();

app.UseHttpsRedirection();
app.UseStaticFiles();

// Rutas y middleware
app.UseRouting();

app.UseAuthentication(); // Activar autenticación
app.UseAuthorization();  // Activar autorización

// Rutas personalizadas
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=RedirectByRole}/{id?}");

// Iniciar la aplicación
app.Run();
