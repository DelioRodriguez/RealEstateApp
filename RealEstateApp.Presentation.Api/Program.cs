using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.IdentityModel.Tokens;
using RealEstateApp.Application;
using RealEstateApp.Infrastructure.Identity;
using RealEstateApp.Infrastructure.Persistance;
using RealEstateApp.Presentation.Api5.Extensions;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Conf JWT
var jwtSettings = builder.Configuration.GetSection("JWTSettings");
var secretKey = jwtSettings.GetValue<string>("Key");

// Conf de aut JWT
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = "JwtBearer";
    options.DefaultChallengeScheme = "JwtBearer";
})
.AddJwtBearer("JwtBearer", options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings.GetValue<string>("Issuer"),
        ValidAudience = jwtSettings.GetValue<string>("Audience"),
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
        ClockSkew = TimeSpan.Zero
    };
});

// Registrar servicios de infraestructura y app
builder.Services.AddIdentityService();
builder.Services.AddIdentityInfrastructure(builder.Configuration);
builder.Services.AddContextInfrastructure(builder.Configuration);
builder.Services.AddApplicationService();

// Conf de CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", corsPolicyBuilder =>
        corsPolicyBuilder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});

// Servicios
builder.Services.AddControllers();

// Confi de Swagger
builder.Services.AddSwaggerExtension();

// Conf de ver de API en Swagger
builder.Services.AddApiVersionExtension();

// Agregar otros servicios como health checks, etc.
builder.Services.AddHealthChecks();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();

var app = builder.Build();

// Conf del middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Iniciar Usuarios
await app.Services.RunIdentitySeeds();

app.UseRouting();
app.UseHttpsRedirection();

// Confi de CORS
app.UseCors("AllowAll");

// Confi de aut y auto
app.UseAuthentication();
app.UseAuthorization();

// Manejo de errores
app.UseStatusCodePages(context =>
{
    if (context.HttpContext.Response.StatusCode == StatusCodes.Status401Unauthorized)
    {
        return context.HttpContext.Response.WriteAsync("No estás autorizado para acceder a este recurso.");
    }

    if (context.HttpContext.Response.StatusCode == StatusCodes.Status403Forbidden)
    {
        return context.HttpContext.Response.WriteAsync("Acceso denegado.");
    }

    return Task.CompletedTask;
});

// Confi de Swagger UI con soporte para versiones
var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
app.UseSwaggerExtension(provider);

app.UseHealthChecks("/health");

app.MapControllers();

app.Run();
