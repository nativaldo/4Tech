using Health.Domain.Shared.Abstractions;

namespace Health.Application.UseCases.Beneficiarios.SolicitacaoDeleteBeneficiario;

public record SolicitacaoDeleteBeneficiarioRequest(Guid Id, int Prioridade) : IMessage<SolicitacaoDeleteBeneficiarioResponse>;
