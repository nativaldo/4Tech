namespace Health.Domain.Shared.Abstractions.Dtos;

// Este é o "contrato raiz" que todos conhecem
public record BeneficiarioDto(
    Guid Id,
    string NomeCompleto,
    string Cpf,
    DateTime DataNascimento,
    int Status,
    string Plano,
    int TotalCount
);