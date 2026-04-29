using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Models;

namespace Health.API.Extensions;

public static class SwaggerConfig
{
    public static IServiceCollection AddSwaggerConfig(this IServiceCollection services)
    {
        services.AddOpenApi(options =>
        {
            options.AddDocumentTransformer((document, context, cancellationToken) =>
            {
                document.Info.Title = "Health Management API";
                document.Info.Version = "v1";
                document.Info.Description = "Documentação via Swagger + OpenAPI - Nativaldo";

                // Configuração de Segurança (Bearer JWT)
                var bearerScheme = new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header usando o esquema Bearer."
                };

                document.Components ??= new OpenApiComponents();
                document.Components.SecuritySchemes.Add("Bearer", bearerScheme);
                document.SecurityRequirements = new List<OpenApiSecurityRequirement>
                {
                    new() { { new OpenApiSecurityScheme { Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" } }, Array.Empty<string>() } }
                };

                return Task.CompletedTask;
            });
        });

        return services;
    }

    public static IApplicationBuilder UseSwaggerConfig(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            // 1. Gera o endpoint do JSON nativo (/openapi/v1.json)
            app.MapOpenApi();

            // 2. Configura a UI do Swagger para ler o JSON nativo
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/openapi/v1.json", "Health API v1");
                options.RoutePrefix = "swagger"; // Acesso via /swagger
            });
        }

        return app;
    }
}