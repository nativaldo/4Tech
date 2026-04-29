namespace Health.Domain.Shared.Abstractions.Common.Errors;
// Record para representar cada falha em um campo específico
public record ErrorDetail(string Field, string Rule, string Message = "");