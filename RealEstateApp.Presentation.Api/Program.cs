using Microsoft.AspNetCore.Mvc.ApiExplorer;
using RealEstateApp.Application;
using RealEstateApp.Infrastructure.Identity;
using RealEstateApp.Infrastructure.Persistance;
using RealEstateApp.Presentation.Api5.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddIdentityInfrastructure(builder.Configuration);
builder.Services.AddContextInfrastructure(builder.Configuration);
builder.Services.AddApplicationService();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", corsPolicyBuilder =>
        corsPolicyBuilder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});

// Add services to the container.
builder.Services.AddIdentityInfrastructure(builder.Configuration);

builder.Services.AddControllers();

builder.Services.AddSwaggerExtension();
builder.Services.AddApiVersionExtension();
builder.Services.AddHealthChecks();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();

app.UseHttpsRedirection();

app.UseCors("AllowAll");

var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
app.UseSwaggerExtension(provider);

app.UseHealthChecks("/health");
app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();


app.MapControllers();

app.Run();