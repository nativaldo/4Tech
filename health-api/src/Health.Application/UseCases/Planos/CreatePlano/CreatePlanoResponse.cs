using Health.Domain.Shared.Abstractions;

namespace Health.Application.UseCases.Beneficiarios.CreatePlano;

public record CreatePlanoResponse(Guid Id) : IUseCaseResponse;