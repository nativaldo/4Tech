namespace Health.Domain.Shared.Abstractions.DTOs;

public record PlanoDto(
    Guid Id,
    string Nome,
    string CodigoRegistroAns
);