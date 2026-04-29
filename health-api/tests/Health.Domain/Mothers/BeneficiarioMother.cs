using Health.Domain.Entities;
using Health.Domain.Shared.Abstractions.Dtos;

namespace Health.UnitTests.Domain.Mothers;

public static class BeneficiarioMother
{
    public static Beneficiario Valido() =>
        Beneficiario.Create(
            "Fulano de Tal",
            "12345678901",
            DateTime.Today.AddYears(-30),
            Guid.NewGuid()).Value!;
}
