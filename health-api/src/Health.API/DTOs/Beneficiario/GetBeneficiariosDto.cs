namespace Health.API.DTOs.Beneficiario;

public record GetBeneficiariosDto(
    int? Page = 1,
    int? PageSize = 10,
    string? Search = null);
