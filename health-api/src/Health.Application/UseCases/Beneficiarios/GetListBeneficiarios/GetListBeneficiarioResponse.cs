using Health.Domain.Shared.Abstractions;
using Health.Domain.Shared.Abstractions.Dtos;

namespace Health.Application.UseCases.Beneficiarios.GetListBeneficiarios;

public record GetListBeneficiariosResponse : IUseCaseResponse
{
    private string nomeCompleto;
    private string cpf;
    private string dataNascimento;
    private string status;
    private string plano;
    private Guid id;

    public GetListBeneficiariosResponse(Guid Id, string NomeCompleto, string Cpf, string DataNascimento, string Status, string Plano)
    {
        id = Id;
        nomeCompleto = NomeCompleto;
        cpf = Cpf;
        dataNascimento = DataNascimento;
        status = Status;
        plano = Plano;
    }

    // Factory Method para converter a Entidade de Domínio em DTO de Resposta
    public static GetListBeneficiariosResponse FromEntity(BeneficiarioPageDto beneficiario)
    {
        return new GetListBeneficiariosResponse(
            Id: beneficiario.Id,
            NomeCompleto: beneficiario.NomeCompleto,
            Cpf: beneficiario.Cpf,
            DataNascimento: beneficiario.DataNascimento.ToString("yyyy-MM-dd"),
            Status: beneficiario.Status.ToString().ToUpper(),
            Plano: beneficiario.NomeCompleto ?? "N/A"
        );
    }
}