namespace Health.API.DTOs.Plano;

public record GetListPlanosDto(
    int? Page = 1,
    int? PageSize = 10,
    string? Search = null);