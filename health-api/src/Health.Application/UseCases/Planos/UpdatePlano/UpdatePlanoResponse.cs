using Health.Domain.Shared.Abstractions;

namespace Health.Application.UseCases.Planos.UpdatePlano;

public record UpdatePlanoResponse(
    Guid Id,
    string Nome,
    string CodigoRegistroAns
) : IUseCaseResponse;