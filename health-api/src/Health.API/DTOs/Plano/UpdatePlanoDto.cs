namespace Health.API.DTOs.Plano;

/// <summary>
/// DTO de entrada para a atualização de um Plano de Saúde.
/// </summary>
public record UpdatePlanoDto(
    string Nome,
    string CodigoRegistroAns
);