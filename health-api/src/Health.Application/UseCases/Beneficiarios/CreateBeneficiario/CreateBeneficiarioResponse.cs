using Health.Domain.Shared.Abstractions;

namespace Health.Application.UseCases.Beneficiarios.CreateBeneficiario;

public record CreateBeneficiarioResponse(Guid Id) : IUseCaseResponse;