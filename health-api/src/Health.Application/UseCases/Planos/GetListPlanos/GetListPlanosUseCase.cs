using Health.Domain.Shared.Abstractions;
using Health.Domain.Shared.Abstractions.Common;
using Health.Application.UseCases.Beneficiarios.GetListBeneficiarios;
using Health.Domain.Interfaces;
using Health.Domain.Errors;
using Health.Domain.Shared.Enums;
using Health.Domain.Shared.Abstractions.Common.Errors;
using Health.Application.UseCases.Planos.GetListPlanos;
using Health.Application.UseCases.Beneficiarios.GetPlano;
using Health.Domain.Shared.Abstractions.DTOs;

namespace Health.Application.UseCases.Beneficiarios.GetList;

/// <summary>
/// Use Case de listagem seguindo o padrão de Primary Constructor e Interface Base.
/// </summary>
public sealed class GetListPlanosUseCase(
    IPlanoRepository _readRepository)
    : IUseCase<GetListPlanosRequest, GetListPlanosResponse>
{
    public async Task<Result<GetListPlanosResponse>> ExecuteAsync(
        GetListPlanosRequest request,
        CancellationToken ct = default)
    {
        var planos = await _readRepository.GetAllAsync(ct);

        if (planos is null || !planos.Any())
            return Result<GetListPlanosResponse>.Failure(ErrorType.NotFound, ErrorRegistry.Domain.Plano.NenhumRegistroEncontrado);

        // 1. Mapeia os DTOs do banco para os itens da resposta
        var itens = planos.Select(dto => new PlanoDto(
            Id: dto.Id,
            Nome: dto.Nome,
            CodigoRegistroAns: dto.CodigoRegistroAns
        ));

        var response = new GetListPlanosResponse(itens);

        return Result<GetListPlanosResponse>.Success(response);
    }
}