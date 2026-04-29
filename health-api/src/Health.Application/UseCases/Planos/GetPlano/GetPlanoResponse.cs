using Health.Domain.Shared.Abstractions;

namespace Health.Application.UseCases.Planos.GetPlano;

public record GetPlanoResponse(
    Guid Id,
    string Nome,
    string CodigoRegistroAns
) : IUseCaseResponse;