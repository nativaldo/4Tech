using Health.Domain.Enums;
using Health.Domain.Shared.Abstractions.Dtos;
using Health.Domain.Shared.Enums;

namespace Health.UnitTests.Domain.Mothers;

public static class BeneficiarioDtoMother
{
    public static BeneficiarioPageDto RecuperarValido() =>
        new(
            Id: Guid.NewGuid(),
            NomeCompleto: "Fulano de Tal",
            Cpf: "12345678901",
            DataNascimento: new DateTime(1994, 05, 20),
            Plano: "Plano Vip",
            Status: EStatusBeneficiario.ATIVO.ToString(),
            TotalCount: 1
        );

    public static (IEnumerable<BeneficiarioPageDto> Items, int TotalCount) PaginaComUmItem()
    {
        var lista = new List<BeneficiarioPageDto> { RecuperarValido() };
        return (lista, 1);
    }
}