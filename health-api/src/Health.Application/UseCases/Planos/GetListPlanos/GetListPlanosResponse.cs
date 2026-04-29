using Health.Domain.Shared.Abstractions;
using Health.Domain.Shared.Abstractions.DTOs;

namespace Health.Application.UseCases.Planos.GetListPlanos;

public record GetListPlanosResponse(IEnumerable<PlanoDto> Planos) : IUseCaseResponse;
