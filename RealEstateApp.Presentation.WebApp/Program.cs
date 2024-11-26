using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Application;
using RealEstateApp.Application.Settings;
using RealEstateApp.Infrastructure.Identity;
using RealEstateApp.Infrastructure.Persistance;
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

// Agregar ApiVersioning y VersionedApiExplorer // Nuevola
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
});

builder.Services.AddVersionedApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

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
    pattern: "{controller=Properties}/{action=Index}/{id?}");

app.Run();