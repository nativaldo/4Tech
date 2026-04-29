using Health.Domain.Shared.Abstractions;
using System.Linq;

namespace Health.Application.UseCases.Beneficiarios.CreateBeneficiario;

// Removemos os parâmetros da assinatura da classe para usar o construtor explícito
public record CreateBeneficiarioRequest : IMessage<CreateBeneficiarioResponse>
{
    public string Nome { get; init; }
    public string Cpf { get; init; }
    public string Email { get; init; }
    public DateTime DataNascimento { get; init; }
    public Guid PlanoId { get; init; }

    // Construtor privado: impede a criação direta fora desta classe
    private CreateBeneficiarioRequest(
        string nome,
        string cpf,
        string email,
        DateTime dataNascimento,
        Guid planoId)
    {
        Nome = nome;
        Cpf = cpf;
        Email = email;
        DataNascimento = dataNascimento;
        PlanoId = planoId;
    }

    /// <summary>
    /// Factory Method: O único ponto de entrada para este Request.
    /// Garante que os dados entrem limpos (sanitizados).
    /// </summary>
    public static CreateBeneficiarioRequest Create(
        string nome,
        string cpf,
        string email,
        DateTime dataNascimento,
        Guid planoId)
    {
        // Sanitização (Normalização de dados)
        var nomeNormalizado = nome?.Trim() ?? string.Empty;
        var cpfLimpo = new string(cpf?.Where(char.IsDigit).ToArray() ?? []);
        var emailNormalizado = email?.Trim().ToLower() ?? string.Empty;

        return new CreateBeneficiarioRequest(
            nomeNormalizado,
            cpfLimpo,
            emailNormalizado,
            dataNascimento,
            planoId);
    }
}