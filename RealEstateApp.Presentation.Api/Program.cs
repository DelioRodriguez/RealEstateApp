using Asp.Versioning.ApiExplorer;
using RealEstateApp.Infrastructure.Identity;
using RealEstateApp.Presentation.Api5.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Registrar servicios de infraes y app
builder.Services.AddIdentityInfrastructureApi(builder.Configuration);

// Conf de JWT
builder.Services.AddAuthenticationExtension(builder.Configuration);

// Conf de CORS
builder.Services.AddCorsExtension();

// Servicios de Swagger
builder.Services.AddSwaggerExtension();

// Conf de versión de la API
builder.Services.AddApiVersioningExtension();

// Otros servicios
builder.Services.AddControllers();
builder.Services.AddHealthChecks();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();

var app = builder.Build();

// Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Iniciar usuarios
await app.Services.RunIdentitySeedsApi();

app.UseRouting();
app.UseHttpsRedirection();

// Conf de CORS
app.UseCors("AllowAll");

// Autenticación y autorización
app.UseAuthentication();
app.UseAuthorization();

// Manejo de errores
app.UseStatusCodePages(context =>
{
    if (context.HttpContext.Response.StatusCode == StatusCodes.Status401Unauthorized)
    {
        return context.HttpContext.Response.WriteAsync("No estas autorizado para acceder a este recurso.");
    }

    if (context.HttpContext.Response.StatusCode == StatusCodes.Status403Forbidden)
    {
        return context.HttpContext.Response.WriteAsync("Acceso denegado.");
    }

    return Task.CompletedTask;
});

// Confi de Swagger UI con soporte para versiones
var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
app.UseSwaggerExtension(app);

app.UseHealthChecks("/health");

app.MapControllers();

app.Run();
