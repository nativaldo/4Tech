using Health.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Health.Infrastructure.Adapters.Postgres.Mappings;

public class ExclusaoBeneficiarioFilaConfiguration : IEntityTypeConfiguration<ExclusaoBeneficiarioFila>
{
    public void Configure(EntityTypeBuilder<ExclusaoBeneficiarioFila> builder)
    {
        builder.ToTable("ExclusaoBeneficiarioFila");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.BeneficiarioId)
            .IsRequired();

        builder.Property(e => e.Prioridade)
            .IsRequired();

        builder.Property(e => e.DataSolicitacao)
            .IsRequired();

        builder.Property(e => e.IsDeleted)
            .HasDefaultValue(false);

        // Índice para acelerar a busca pela fila de prioridade
        builder.HasIndex(e => new { e.Prioridade, e.DataSolicitacao })
            .HasDatabaseName("IX_FilaExclusao_Prioridade_Data");
    }
}