
using Health.Domain.Interfaces;
using Health.Domain.Errors;
using Health.Domain.Shared.Abstractions;
using Health.Domain.Shared.Enums;

namespace Health.Application.UseCases.Beneficiarios.UpdateBeneficiario;

public sealed class UpdateBeneficiarioCase(
    IBeneficiarioRepository _beneficiarioRepository,
    IUnitOfWork _unitOfWork) : IUseCase<UpdateBeneficiarioRequest, UpdateBeneficiarioResponse>
{
    public async Task<Result<UpdateBeneficiarioResponse>> ExecuteAsync(UpdateBeneficiarioRequest request, CancellationToken ct)
    {
        var beneficiario = await _beneficiarioRepository.GetByIdAsync(request.Id, ct);

        // Validação de existência
        if (beneficiario is null)
            return Result<UpdateBeneficiarioResponse>.Failure(ErrorType.NotFound, ErrorRegistry.Domain.Beneficiario.NaoEncontrado);

        beneficiario.AtualizarBeneficiario(
        request.Email,
        request.Status
        );

        await _beneficiarioRepository.UpdateAsync(beneficiario, ct);

        var isSuccess = await _unitOfWork.CommitAsync(ct);
        if (!isSuccess)
            return Result<UpdateBeneficiarioResponse>.Failure(ErrorType.Internal, ErrorRegistry.Domain.Common.CommitError);

        return Result<UpdateBeneficiarioResponse>.Success(new UpdateBeneficiarioResponse());
    }
}