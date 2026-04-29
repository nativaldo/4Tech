using Health.Domain.Entities;
using Health.Domain.Interfaces;
using Health.Domain.Errors;
using Health.Domain.Shared.Abstractions;
using Health.Domain.Shared.Enums;



namespace Health.Application.UseCases.Beneficiarios.CreateBeneficiario;

public sealed class CreateBeneficiarioUseCase(
    IBeneficiarioRepository _beneficiarioRepository,
    IPlanoRepository _planoRepository,
    IUnitOfWork _unitOfWork) : IUseCase<CreateBeneficiarioRequest, CreateBeneficiarioResponse>
{
    public async Task<Result<CreateBeneficiarioResponse>> ExecuteAsync(CreateBeneficiarioRequest request, CancellationToken ct)
    {
        // Regra de Domínio: CPF Único
        if (await _beneficiarioRepository.AnyWithCpfAsync(request.Cpf, ct))
            return Result<CreateBeneficiarioResponse>.Failure(ErrorType.Conflict, ErrorRegistry.Domain.Beneficiario.CpfExistente);

        // Regra de Domínio: Existência do Plano
        if (!await _planoRepository.ExistsAsync(request.PlanoId, ct))
            return Result<CreateBeneficiarioResponse>.Failure(ErrorType.NotFound, ErrorRegistry.Domain.Plano.NaoEncontrado);

        var beneficiarioResult = Beneficiario.Create(
            request.Nome,
            request.Cpf,
            DateTime.SpecifyKind(request.DataNascimento, DateTimeKind.Utc),
            request.PlanoId);

        await _beneficiarioRepository.AddAsync(beneficiarioResult.Value!, ct);

        var isSuccess = await _unitOfWork.CommitAsync(ct);

        if (!isSuccess)
            return Result<CreateBeneficiarioResponse>.Failure(ErrorType.Internal, ErrorRegistry.Domain.Common.CommitError);

        return Result<CreateBeneficiarioResponse>.Success(new CreateBeneficiarioResponse(beneficiarioResult.Value!.Id));
    }
}