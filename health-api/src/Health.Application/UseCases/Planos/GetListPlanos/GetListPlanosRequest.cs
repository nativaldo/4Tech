using Health.Application.UseCases.Planos.GetListPlanos;
using Health.Domain.Shared.Abstractions;

public record GetListPlanosRequest() : IMessage<GetListPlanosResponse>;