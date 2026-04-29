
using Health.Domain.Interfaces;
using Health.Domain.Errors;
using Health.Domain.Shared.Abstractions;
using Health.Domain.Shared.Enums;
using Health.Domain.Enums;

namespace Health.Application.UseCases.Beneficiarios.GetBeneficiario;

public sealed class GetBeneficiarioCase(IBeneficiarioRepository _beneficiarioRepository) : IUseCase<GetBeneficiarioRequest, GetBeneficiarioResponse>
{
    public async Task<Result<GetBeneficiarioResponse>> ExecuteAsync(GetBeneficiarioRequest request, CancellationToken ct)
    {
        var beneficiario = await _beneficiarioRepository.GetByIdAsync(request.Id, ct);

        // Validação de existência
        if (beneficiario is null)
            return Result<GetBeneficiarioResponse>.Failure(ErrorType.NotFound, ErrorRegistry.Domain.Beneficiario.NaoEncontrado);

        var response = new GetBeneficiarioResponse(
            beneficiario.NomeCompleto,
            beneficiario.Cpf,
            beneficiario.DataNascimento,
            beneficiario.PlanoId,
            Enum.GetName(typeof(EStatusBeneficiario), beneficiario.Status) ?? "N/A"
        );

        return Result<GetBeneficiarioResponse>.Success(response);
    }
}
