using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.OpenApi.Models;

namespace RealEstateApp.Presentation.Api5.Extensions
{
    public static class ServiceExtension
    {
        public static void AddSwaggerExtension(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                List<string> xmlFiles = Directory.GetFiles(AppContext.BaseDirectory, "*.xml", searchOption: SearchOption.TopDirectoryOnly).ToList();
                xmlFiles.ForEach(xmlFile => options.IncludeXmlComments(xmlFile));

                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1.0",
                    Title = "Restaurant API",
                    Description = "Restaurant API",
                    Contact = new OpenApiContact
                    {
                        Name = "Ivo Luis Rodriguez",
                        Email = "20231197@itla.edu.do",
                        Url = new Uri("https://www.itla.edu.do")
                    }
                });

                // Ejemplo de la version 2
                options.SwaggerDoc("v2", new OpenApiInfo
                {
                    Version = "v1.0",
                    Title = "Restaurant API",
                    Description = "Restaurant API",
                    Contact = new OpenApiContact
                    {
                        Name = "Ivo Luis Rodriguez",
                        Email = "20231197@itla.edu.do",
                        Url = new Uri("https://www.itla.edu.do")
                    }
                });

                options.DescribeAllParametersInCamelCase();

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    Description = "Input your Bearer token in this format - Bearer {your token here}"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id="Bearer"
                            },
                            Scheme = "Bearer",
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                        }, new List<string>()
                    },
                });
            });
        }

        public static void AddApiVersionExtension(this IServiceCollection services)
        {
            services.AddApiVersioning(config =>
            {
                config.DefaultApiVersion = new ApiVersion(1, 0);
                config.AssumeDefaultVersionWhenUnspecified = true;
                config.ReportApiVersions = false;
                config.ApiVersionReader = ApiVersionReader.Combine(
                    new QueryStringApiVersionReader("api-version"),
                    new HeaderApiVersionReader("x-api-version") 
                );
            });
            
            services.AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";  
                options.SubstituteApiVersionInUrl = true; 
            });
        }
    }
}
