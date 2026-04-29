using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Health.Infrastructure.Adapters.Keycloak;

public static class KeycloakExtensions
{
    public static IServiceCollection AddKeycloakAdapter(this IServiceCollection services, IConfiguration config)
    {
        var url = config["KEYCLOAK_URL"];
        var clientId = config["KEYCLOAK_CLIENT_ID"];
        var secret = config["KEYCLOAK_CLIENT_SECRET"]; // Secret sensível

        if (string.IsNullOrEmpty(url) || string.IsNullOrEmpty(secret))
            throw new InvalidOperationException("Configurações do Keycloak ausentes nos Secrets do container.");

        // Configura o HttpClient ou Middleware de Auth usando esses valores diretamente
        services.AddHttpClient("KeycloakClient", client =>
        {
            client.BaseAddress = new Uri(url);
        });

        return services;
    }
}