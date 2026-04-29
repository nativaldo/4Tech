using Health.Domain.Enums;
using Health.Domain.Shared.Abstractions;

namespace Health.Application.UseCases.Beneficiarios.UpdateBeneficiario;

public record UpdateBeneficiarioRequest : IMessage<UpdateBeneficiarioResponse>
{
    // O ID vem da Rota (/beneficiarios/editar/{id})
    public Guid Id { get; private set; }
    // Os demais campos vêm do Body (JSON)
    public string Nome { get; private set; }
    public string Cpf { get; private set; }
    public string Email { get; private set; }
    public DateTime DataNascimento { get; private set; }
    public Guid PlanoId { get; private set; }
    public EStatusBeneficiario Status { get; private set; }

    // Construtor privado para forçar o uso do Create
    private UpdateBeneficiarioRequest(
        Guid id,
        string nome,
        string email,
        DateTime dataNascimento,
        EStatusBeneficiario status)
    {
        Id = id;
        Nome = nome;
        Email = email;
        DataNascimento = dataNascimento;
        Status = status;
    }

    /// <summary> Nativaldo ToDo
    /// Factory Method para montagem do Request.
    /// Útil para sanitização inicial de strings e logs.
    /// </summary>
    public static UpdateBeneficiarioRequest Create(
        Guid id,
        string nome,
        string email,
        DateTime dataNascimento,
        EStatusBeneficiario status)
    {
        return new UpdateBeneficiarioRequest(
            id,
            nome?.Trim() ?? string.Empty,
            email?.ToLower().Trim() ?? string.Empty,
            dataNascimento,
            status);
    }
}