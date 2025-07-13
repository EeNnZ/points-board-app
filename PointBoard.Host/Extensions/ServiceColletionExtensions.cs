using System.Reflection;
using Microsoft.OpenApi.Models;

namespace PointBoard.Host.Extensions;

/// <summary>
/// Provides extension methods for service collection configuration.
/// </summary>
public static class ServiceColletionExtensions
{
    /// <summary>
    /// Adds default Swagger/OpenAPI configuration to the service collection.
    /// </summary>
    /// <param name="services">The service collection to configure.</param>
    public static IServiceCollection AddDefaultSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(op =>
        {
            op.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = $"{Properties.Resources.SolutionName} API",
                Description = Properties.Resources.ApiDescription,
                TermsOfService = new Uri("https://example.com/terms"),
                Contact = new OpenApiContact
                {
                    Name = "Example Contact",
                    Url = new Uri("https://example.com/contact")
                },
                License = new OpenApiLicense
                {
                    Name = "Example License",
                    Url = new Uri("https://example.com/license")
                }
            });

            var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            op.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
        });

        return services;
    }
}