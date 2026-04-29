using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Health.Infrastructure.Adapters.Postgres.Extensions;

public static class RepositoryExtensions
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        // 1. Identifica o Assembly onde estão as implementações dos repositórios
        var infrastructureAssembly = Assembly.GetExecutingAssembly();

        // 2. Procura todas as classes concretas que terminam com "Repository"
        var repositoryRegistrations = infrastructureAssembly.GetTypes()
            .Where(t => t.Name.EndsWith("Repository")
                        && !t.IsInterface
                        && !t.IsAbstract)
            .Select(implementation => new
            {
                Implementation = implementation,
                // Tenta encontrar a interface correspondente (ex: IBeneficiarioRepository)
                Interface = implementation.GetInterfaces()
                    .FirstOrDefault(i => i.Name == $"I{implementation.Name}")
            })
            .Where(x => x.Interface != null);

        foreach (var repo in repositoryRegistrations)
        {
            services.AddScoped(repo.Interface!, repo.Implementation);
        }

        return services;
    }
}