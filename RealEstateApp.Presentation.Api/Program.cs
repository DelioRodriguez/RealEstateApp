using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Infrastructure.Identity;
using RealEstateApp.Presentation.Api5.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Conf Controladores para devolver Json
builder.Services.AddControllers(options =>
{
    options.Filters.Add(new ProducesAttribute("application/json"));
}).ConfigureApiBehaviorOptions(options =>
{
    options.SuppressInferBindingSourcesForParameters = true;
    options.SuppressMapClientErrors = true;
});

builder.Services.AddInfrastructureServicesApi(builder.Configuration);
builder.Services.AddAuthenticationExtension(builder.Configuration);
builder.Services.AddCorsExtension();
builder.Services.AddSwaggerExtension(); 
builder.Services.AddApiVersioningExtension();
builder.Services.AddHealthChecks();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwaggerExtension(app);
}

// init identity
await app.Services.RunIdentitySeedsApi();

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseStatusCodePages(context =>
{
    if (context.HttpContext.Response.StatusCode == StatusCodes.Status401Unauthorized)
    {
        return context.HttpContext.Response.WriteAsync("No est�s autorizado para acceder a este recurso.");
    }

    if (context.HttpContext.Response.StatusCode == StatusCodes.Status403Forbidden)
    {
        return context.HttpContext.Response.WriteAsync("Acceso denegado.");
    }

    return Task.CompletedTask;
});

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.UseHealthChecks("/health");
app.UseSession();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();