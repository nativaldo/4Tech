using Health.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Health.Infrastructure.Adapters.Postgres.Mappings;

public sealed class BeneficiarioMapping : IEntityTypeConfiguration<Beneficiario>
{
    public void Configure(EntityTypeBuilder<Beneficiario> builder)
    {
        builder.ToTable("Beneficiarios");

        builder.HasKey(b => b.Id);
        builder.Property(x => x.Id)
               .ValueGeneratedNever();

        builder.Property(b => b.NomeCompleto)
            .IsRequired()
            .HasMaxLength(150);

        // Garante CPF Único no Banco de Dados
        builder.Property(b => b.Cpf)
            .IsRequired()
            .HasMaxLength(11)
            .IsFixedLength();

        builder.HasIndex(b => b.Cpf)
            .IsUnique();

        builder.Property(b => b.Status)
            .HasConversion<int>()
            .IsRequired();

        // Integridade Referencial (FK)
        builder.HasOne<Plano>()
            .WithMany()
            .HasForeignKey(b => b.PlanoId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}