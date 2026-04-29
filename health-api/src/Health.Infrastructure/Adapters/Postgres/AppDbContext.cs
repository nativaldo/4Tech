using Health.Domain.Core.Models;
using Health.Domain.Entities;
using Health.Infrastructure.Adapters.Postgres.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Health.Infrastructure.Adapters.Postgres;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Beneficiario> Beneficiarios => Set<Beneficiario>();
    public DbSet<Plano> Planos => Set<Plano>();
    public DbSet<ExclusaoBeneficiarioFila> ExclusaoBeneficiarioFila { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        modelBuilder.ApplySnakeCaseNaming();

        const string Prefixo = "hth_";

        foreach (var entity in modelBuilder.Model.GetEntityTypes())
        {
            var tableName = entity.GetTableName();

            if (!string.IsNullOrEmpty(tableName) && !tableName.StartsWith(Prefixo))
            {
                entity.SetTableName($"{Prefixo}{tableName}");
            }
        }
        modelBuilder.Entity<Plano>().ToTable("hth_planos");
        modelBuilder.Entity<Beneficiario>().ToTable("hth_beneficiarios");
        modelBuilder.Entity<ExclusaoBeneficiarioFila>().ToTable("hth_exclusao_beneficiario_fila");

        base.OnModelCreating(modelBuilder);
    }
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return base.SaveChangesAsync(cancellationToken);
    }
}