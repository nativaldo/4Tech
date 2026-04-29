using Health.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Health.Infrastructure.Adapters.Postgres.Mappings;

public sealed class PlanoMapping : IEntityTypeConfiguration<Plano>
{
    public void Configure(EntityTypeBuilder<Plano> builder)
    {
        builder.ToTable("planos");

        builder.HasKey(p => p.Id);

        builder.Property(x => x.Id)
               .ValueGeneratedNever(); // Informa que o ID já vem preenchido do código

        // Requisito: Nome obrigatório e único
        builder.Property(p => p.Nome)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasIndex(p => p.Nome)
            .IsUnique();

        // Requisito: Código ANS obrigatório e único
        builder.Property(p => p.CodigoRegistroAns)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(p => p.CodigoRegistroAns)
            .IsUnique();
    }
}