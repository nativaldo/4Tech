using Health.Application.UseCases.Beneficiarios.Create;
using Health.Application.UseCases.Beneficiarios.CreateBeneficiario;

namespace Health.UnitTests.Application.Mothers.BeneficiarioMother;

public static class BeneficiarioRequestMother
{
    public static CreateBeneficiarioRequest Valido() =>
        CreateBeneficiarioRequest.Create(
            "Fulano de Tal",
            "123.456.789-00", // O Factory Method do Request vai sanitizar isso
            "fulano@health.com.br",
            DateTime.Today.AddYears(-30),
            Guid.NewGuid());

    public static CreateBeneficiarioRequest ComCpfInvalido() =>
        CreateBeneficiarioRequest.Create(
            "Fulano Errado",
            "000",
            "erro@health.com",
            DateTime.Today.AddYears(-30),
            Guid.NewGuid());

    public static CreateBeneficiarioRequest MenorDeIdade() =>
        CreateBeneficiarioRequest.Create(
            "Criança",
            "98765432100",
            "kids@health.com",
            DateTime.Today.AddYears(-5),
            Guid.NewGuid());
}