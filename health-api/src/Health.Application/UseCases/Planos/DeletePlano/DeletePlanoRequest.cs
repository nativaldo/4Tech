using Health.Domain.Shared.Abstractions;

namespace Health.Application.UseCases.Planos.DeletePlano;

public record DeletePlanoRequest(Guid id) : IMessage<DeletePlanoResponse>;


