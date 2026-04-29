
using Health.Domain.Interfaces;
using Health.Domain.Errors;
using Health.Domain.Shared.Abstractions;
using Health.Domain.Shared.Enums;

namespace Health.Application.UseCases.Planos.UpdatePlano;

public sealed class UpdatePlanoUseCase(
    IPlanoRepository _planoRepository,
    IUnitOfWork _unitOfWork) : IUseCase<UpdatePlanoRequest, UpdatePlanoResponse>
{
    public async Task<Result<UpdatePlanoResponse>> ExecuteAsync(UpdatePlanoRequest request, CancellationToken ct)
    {
        var plano = await _planoRepository.GetByIdAsync(request.Id, ct);

        // Validação de existência
        if (plano is null)
            return Result<UpdatePlanoResponse>.Failure(ErrorType.NotFound, ErrorRegistry.Domain.Plano.NaoEncontrado);

        plano.UpdateEntity(request.Nome, request.CodigoRegistroAns);

        await _planoRepository.UpdateAsync(plano, ct);

        var isSuccess = await _unitOfWork.CommitAsync(ct);

        if (!isSuccess)
            return Result<UpdatePlanoResponse>.Failure(ErrorType.Internal, ErrorRegistry.Domain.Common.CommitError);

        return Result<UpdatePlanoResponse>.Success(new UpdatePlanoResponse(plano.Id, plano.Nome, plano.CodigoRegistroAns));
    }
}