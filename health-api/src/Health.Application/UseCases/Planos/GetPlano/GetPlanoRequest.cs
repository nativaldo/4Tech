using Health.Application.UseCases.Planos.GetPlano;
using Health.Domain.Shared.Abstractions;

namespace Health.Application.UseCases.Beneficiarios.GetPlano;

public record GetPlanoRequest(Guid id) : IMessage<GetPlanoResponse>;
