using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Health.Domain.Interfaces;
using Health.Infrastructure.Adapters.Postgres.Repositories;
using Health.Infrastructure.Repositories.Beneficiarios;
using Health.Infrastructure.Persistence;
using Health.Domain.Core.Interfaces;
using Health.Infrastructure.Messaging;
using Health.Infrastructure.Repositories;


namespace Health.Infrastructure.Adapters.Postgres.Extensions;

public static class PostgresExtensions
{
    public static IServiceCollection AddPostgresAdapter(this IServiceCollection services, IConfiguration config)
    {

        var connectionString = $"Host={config["HLTH_DB_HOST"] ?? "localhost"};" +
                               $"Port={config["HLTH_DB_PORT"] ?? "5432"};" +
                               $"Database={config["HLTH_DB_NAME"] ?? "health_api_db"};" +
                               $"Username={config["HLTH_DB_USER"] ?? "health_user"};" +
                               $"Password={config["HLTH_DB_PASS"] ?? "health_password"};" +
                               $"Include Error Detail=true";

        // 2. Injeta o DbContext
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(connectionString, b =>
                b.MigrationsAssembly("Health.Infrastructure")));

        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(connectionString));
        // 2. Registro do DbSession 
        // O DI vai injetar o AppDbContext aqui automaticamente
        services.AddScoped<DbSession>();
        services.AddScoped<DapperSession>();

        services.AddScoped<IUnitOfWork, PostgresUnitOfWork>();

        // 3. Repositórios e UoW - Nativaldo ToDo
        services.AddScoped<IUnitOfWork, PostgresUnitOfWork>();
        services.AddScoped<IMediator, CustomMediator>();
        services.AddScoped<IBeneficiarioRepository, BeneficiarioRepository>();
        services.AddScoped<IPlanoRepository, PlanoRepository>();
        services.AddScoped<IExclusaoBeneficiarioRepository, ExclusaoBeneficiarioRepository>();

        // 4. Health Check
        services.AddHealthChecks()
            .AddNpgSql(connectionString);

        return services;
    }
}