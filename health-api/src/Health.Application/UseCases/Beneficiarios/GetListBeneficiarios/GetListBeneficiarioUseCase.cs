using Health.Domain.Shared.Abstractions;
using Health.Domain.Shared.Abstractions.Common;
using Health.Application.UseCases.Beneficiarios.GetListBeneficiarios;
using Health.Domain.Interfaces;
using Health.Domain.Errors;
using Health.Domain.Shared.Enums;
using Health.Domain.Shared.Abstractions.Common.Errors;

namespace Health.Application.UseCases.Beneficiarios.GetList;

/// <summary>
/// Use Case de listagem seguindo o padrão de Primary Constructor e Interface Base.
/// </summary>
public sealed class GetListBeneficiarioUseCase(
    IBeneficiarioRepository _readRepository)
    : IUseCase<GetListBeneficiarioRequest, PagedResult<GetListBeneficiariosResponse>>
{
    public async Task<Result<PagedResult<GetListBeneficiariosResponse>>> ExecuteAsync(
        GetListBeneficiarioRequest request,
        CancellationToken ct = default)
    {
        var page = request.Page <= 0 ? 1 : request.Page;
        var pageSize = request.PageSize <= 0 ? 10 : request.PageSize;

        var (items, totalCount) = await _readRepository.GetPagedAsync(request.Status, page, pageSize, ct);

        if (items is null || !items.Any())
            return Result<PagedResult<GetListBeneficiariosResponse>>.Failure(ErrorType.NotFound, ErrorRegistry.Domain.Plano.NaoEncontrado);

        var responses = items.Select(dto => new GetListBeneficiariosResponse(
            Id: dto.Id,
            NomeCompleto: dto.NomeCompleto,
            Cpf: dto.Cpf,
            DataNascimento: dto.DataNascimento.ToString("yyyy-MM-dd"),
            Status: dto.Status,
            Plano: dto.Plano
        ));

        var pagedResult = PagedResult<GetListBeneficiariosResponse>.Create(
            items: responses,
            page: request.Page,
            pageSize: request.PageSize,
            total: totalCount
        );

        return Result<PagedResult<GetListBeneficiariosResponse>>.Success(pagedResult);
    }
}