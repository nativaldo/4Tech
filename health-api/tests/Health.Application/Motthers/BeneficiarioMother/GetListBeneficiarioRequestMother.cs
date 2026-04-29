using Health.Application.UseCases.Beneficiarios.GetListBeneficiarios;
using Health.Domain.Enums;

namespace Health.UnitTests.Application.Mothers.BeneficiarioMother;

public static class GetListBeneficiarioRequestMother
{
    public static GetListBeneficiarioRequest Padrao() =>
        new(Status: EStatusBeneficiario.ATIVO, Page: 1, PageSize: 10);

    public static GetListBeneficiarioRequest PaginaInvalida() =>
        new(Status: EStatusBeneficiario.ATIVO, Page: 0, PageSize: 0);

    public static GetListBeneficiarioRequest SemFiltro() =>
        new(Status: 0, Page: 1, PageSize: 10);
}