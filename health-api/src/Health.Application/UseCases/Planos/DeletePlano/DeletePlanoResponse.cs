using Health.Domain.Shared.Abstractions;

namespace Health.Application.UseCases.Planos.DeletePlano;

public record DeletePlanoResponse(
 bool Success = true
) : IUseCaseResponse;