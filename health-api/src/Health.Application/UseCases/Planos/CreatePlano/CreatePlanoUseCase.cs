using Health.Domain.Entities;
using Health.Domain.Interfaces;
using Health.Domain.Errors;
using Health.Domain.Enums;
using Health.Domain.Shared.Abstractions;
using Health.Domain.Shared.Enums;
using Health.Application.UseCases.Beneficiarios.CreatePlano;


namespace Health.Application.UseCases.Beneficiarios.Create;

public sealed class CreatePlanoUseCase(
    IPlanoRepository _planoRepository,
    IUnitOfWork _unitOfWork) : IUseCase<CreatePlanoRequest, CreatePlanoResponse>
{
    public async Task<Result<CreatePlanoResponse>> ExecuteAsync(CreatePlanoRequest request, CancellationToken ct)
    {
        // 1. Regra de Domínio: Nome do Plano Único
        if (await _planoRepository.ExistsByNomeAsync(request.Nome, ct))
            return Result<CreatePlanoResponse>.Failure(ErrorType.Conflict, ErrorRegistry.Domain.Plano.NomeDuplicado);

        // 2. Regra de Domínio: Código ANS Único
        if (await _planoRepository.ExistsByCodigoAnsAsync(request.CodigoRegistroAns, ct))
            return Result<CreatePlanoResponse>.Failure(ErrorType.Conflict, ErrorRegistry.Domain.Plano.CodigoAnsDuplicado);

        var plano = Plano.Create(
            request.Nome,
            request.CodigoRegistroAns
        );

        await _planoRepository.AddAsync(plano, ct);
        var isSuccess = await _unitOfWork.CommitAsync(ct);

        if (!isSuccess)
            return Result<CreatePlanoResponse>.Failure(ErrorType.Internal, ErrorRegistry.Domain.Common.CommitError);

        return Result<CreatePlanoResponse>.Success(new CreatePlanoResponse(plano.Id));
    }
}