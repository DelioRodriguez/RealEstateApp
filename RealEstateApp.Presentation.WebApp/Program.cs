using RealEstateApp.Application;
using RealEstateApp.Application.Interfaces.Repositories.DashBoard;
using RealEstateApp.Application.Interfaces.Services.Dashboard;
using RealEstateApp.Application.Services.Dashboard;
using RealEstateApp.Application.Settings;
using RealEstateApp.Infrastructure.Identity;
using RealEstateApp.Infrastructure.Persistance;
using RealEstateApp.Infrastructure.Persistance.Repositories.Dashboard;
using RealEstateApp.Infrastructure.Shared;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddIdentityInfrastructure(builder.Configuration);
builder.Services.AddContextInfrastructure(builder.Configuration);
builder.Services.AddApplicationService();

builder.Services.Configure<SmtpSettings>(
    builder.Configuration.GetSection("SmtpSettings"));

builder.Services.AddSharedService();
builder.Services.AddIdentityService();
builder.Services.AddControllersWithViews();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

await app.Services.RunIdentitySeeds();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); 
app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=RedirectByRole}/{id?}");

app.Run();