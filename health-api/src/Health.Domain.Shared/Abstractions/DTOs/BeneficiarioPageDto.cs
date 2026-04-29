namespace Health.Domain.Shared.Abstractions.Dtos;

// Este é o "contrato raiz" que todos conhecem
public record BeneficiarioPageDto(
    Guid Id,
    string NomeCompleto,
    string Cpf,
    DateTime DataNascimento,
    string Status,
    string Plano,
    int TotalCount // A infra e o relatório precisam dele aqui
);