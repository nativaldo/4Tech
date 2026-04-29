using Health.Application.UseCases.Beneficiarios.CreateBeneficiario;
using Health.Domain.Enums;
using Health.Domain.Shared.Abstractions;
using Health.Domain.Shared.Abstractions.Common;

namespace Health.Application.UseCases.Beneficiarios.GetListBeneficiarios;

/// <summary>
/// Request para listagem de beneficiários. 
/// O status ATIVO é aplicado internamente para garantir a regra de negócio.
/// </summary>
public record GetListBeneficiarioRequest(
    EStatusBeneficiario Status,
    int Page = 1,
    int PageSize = 10) : IMessage<PagedResult<GetListBeneficiariosResponse>>
{
    public static GetListBeneficiarioRequest Create(int? page, int? pageSize)
    {
        // 1. Validação de Regra de Negócio / Sanatização
        var validatedPage = (page == null || page < 1) ? 1 : page.Value;

        var validatedPageSize = (pageSize == null || pageSize < 1) ? 10 : pageSize.Value;
        if (validatedPageSize > 100) validatedPageSize = 100;

        // 2. Retorna a instância garantida
        return new GetListBeneficiarioRequest(
            Status: EStatusBeneficiario.ATIVO, // Forçamos o status da regra de negócio
            Page: validatedPage,
            PageSize: validatedPageSize
        );
    }
}