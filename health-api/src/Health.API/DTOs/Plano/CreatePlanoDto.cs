namespace Health.API.v1.DTOs.Plano;

/// <summary>
/// DTO de entrada para a criação de um novo Plano de Saúde.
/// </summary>
public record CreatePlanoDto(
    string Nome,
    string CodigoRegistroAns
);