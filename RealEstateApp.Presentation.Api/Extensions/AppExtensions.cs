using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace RealEstateApp.Presentation.Api5.Extensions
{
    public static class AppExtensions
    {
        public static void UseSwaggerExtension(this IApplicationBuilder app, IApiVersionDescriptionProvider provider)
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    var url = $"/swagger/{description.GroupName}/swagger.json";
                    var name = $"Restaurant Api - {description.GroupName.ToUpperInvariant()}";
                    options.SwaggerEndpoint(url, name);
                }
            });
        }
    }
}