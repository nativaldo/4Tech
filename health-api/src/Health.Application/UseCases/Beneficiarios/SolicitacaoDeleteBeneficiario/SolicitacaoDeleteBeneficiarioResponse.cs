using Health.Domain.Shared.Abstractions;

namespace Health.Application.UseCases.Beneficiarios.SolicitacaoDeleteBeneficiario;

public record SolicitacaoDeleteBeneficiarioResponse(bool Success = true) : IUseCaseResponse;