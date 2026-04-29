using Health.Domain.Shared.Abstractions;

namespace Health.Application.UseCases.Beneficiarios.GetBeneficiario;

public record GetBeneficiarioResponse(
    string Nome,
    string Cpf,
    DateTime DataNascimento,
    Guid PlanoId,
    string Status
) : IUseCaseResponse;

