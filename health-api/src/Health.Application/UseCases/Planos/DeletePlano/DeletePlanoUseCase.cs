using Health.Domain.Errors;
using Health.Domain.Interfaces;
using Health.Domain.Shared.Abstractions;
using Health.Domain.Shared.Enums;

namespace Health.Application.UseCases.Planos.DeletePlano;

public sealed class DeletePlanoUseCase(
    IPlanoRepository planoRepo,
    IUnitOfWork unitOfWork)
    : IUseCase<DeletePlanoRequest, DeletePlanoResponse>
{
    public async Task<Result<DeletePlanoResponse>> ExecuteAsync(
        DeletePlanoRequest request,
        CancellationToken ct)
    {
        // 1. Buscar o plano (EF)
        var plano = await planoRepo.GetByIdAsync(request.Id, ct);

        if (plano is null)
            return Result<DeletePlanoResponse>.Failure(ErrorType.NotFound, ErrorRegistry.Domain.Plano.NaoEncontrado);

        await planoRepo.Delete(plano);

        // O EF Core enviará o comando 'DELETE FROM plano WHERE id = @id' para o Postgres     
        var isSuccess = await unitOfWork.CommitAsync(ct);

        if (!isSuccess)
            return Result<DeletePlanoResponse>.Failure(ErrorType.Internal, ErrorRegistry.Domain.Common.CommitError);

        return Result<DeletePlanoResponse>.Success(new DeletePlanoResponse());
    }
}