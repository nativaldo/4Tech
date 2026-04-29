using Health.Domain.Entities;
using Health.Domain.Errors;
using Health.Domain.Interfaces;
using Health.Domain.Shared.Abstractions;
using Health.Domain.Shared.Enums;

namespace Health.Application.UseCases.Beneficiarios.SolicitacaoDeleteBeneficiario;

public sealed class SolicitacaoDeleteBeneficiarioUseCase(
    IBeneficiarioRepository _beneficiarioRepository,
    IExclusaoBeneficiarioRepository _solicitacaoRepo,
    IUnitOfWork _unitOfWork) : IUseCase<SolicitacaoDeleteBeneficiarioRequest, SolicitacaoDeleteBeneficiarioResponse>
{
    public async Task<Result<SolicitacaoDeleteBeneficiarioResponse>> ExecuteAsync(
        SolicitacaoDeleteBeneficiarioRequest request,
        CancellationToken ct)
    {
        // 1. Validar se já existe na fila (Dapper - Performance no Postgres)
        var jaEstaNaFila = await _solicitacaoRepo.ExisteParaBeneficiarioAsync(request.Id);
        if (jaEstaNaFila)
            return Result<SolicitacaoDeleteBeneficiarioResponse>.Failure(ErrorType.Conflict, ErrorRegistry.Domain.Beneficiario.ExisteNaFila);

        // 2. Buscar o beneficiário (EF - Respeitando o Global Query Filter)
        var beneficiario = await _beneficiarioRepository.GetByIdAsync(request.Id);
        if (beneficiario is null)
            return Result<SolicitacaoDeleteBeneficiarioResponse>.Failure(ErrorType.NotFound, ErrorRegistry.Domain.Beneficiario.NaoEncontrado);

        // 3. Criar a solicitação de exclusão (Fila)       
        var solicitacao = ExclusaoBeneficiarioFila.Create(request.Id, request.Prioridade);

        // 4. Persistência na fila (EF)
        await _solicitacaoRepo.AdicionarFilaAsync(solicitacao, ct);

        // 5. Commit Atômico
        // Após o commit, o Global Query Filter ocultará o beneficiário de todo o sistema
        var isSuccess = await _unitOfWork.CommitAsync(ct);

        if (!isSuccess)
            return Result<SolicitacaoDeleteBeneficiarioResponse>.Failure(ErrorType.Internal, ErrorRegistry.Domain.Common.CommitError);


        return Result<SolicitacaoDeleteBeneficiarioResponse>.Success(new SolicitacaoDeleteBeneficiarioResponse());
    }
}