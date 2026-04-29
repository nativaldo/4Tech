using Health.Application.UseCases.Beneficiarios.GetBeneficiario;

namespace Health.UnitTests.Application.Mothers.BeneficiarioMother;

public static class GetBeneficiarioRequestMother
{
    public static GetBeneficiarioRequest ComId(Guid id) => new(id);

    public static GetBeneficiarioRequest IdAleatorio() => new(Guid.NewGuid());
}