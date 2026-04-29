using Health.Domain.Core.Models;
using Health.Domain.Enums;
using Health.Domain.Shared.Abstractions;

namespace Health.Domain.Entities;

public sealed class Beneficiario : Entity
{
    // Propriedades com Init ou Private Set para garantir Encapsulamento
    public string NomeCompleto { get; private set; }
    public string Cpf { get; private set; }
    public DateTime DataNascimento { get; private set; }
    public Guid PlanoId { get; private set; }
    public DateTime DataCadastro { get; private set; } = DateTime.UtcNow;
    public EStatusBeneficiario Status { get; private set; } = EStatusBeneficiario.ATIVO;
    public bool IsDeleted { get; private set; }

    // Construtor privado para forçar o uso do Factory Method (Create)
    private Beneficiario(string nome, string cpf, DateTime dataNascimento, Guid planoId)
    {
        NomeCompleto = nome;
        Cpf = cpf;
        DataNascimento = dataNascimento;
        PlanoId = planoId;
        IsDeleted = false;
    }

    // Construtor para o EF Core
    private Beneficiario() { }

    /// <summary>
    /// Factory Method para criação de um novo beneficiário.
    /// </summary>
    public static Result<Beneficiario> Create(string nome, string cpf, DateTime dataNascimento, Guid planoId)
    {
        var beneficiario = new Beneficiario(nome, cpf, dataNascimento, planoId);
        return Result<Beneficiario>.Success(beneficiario);
    }

    /// <summary>
    /// Realiza a atualização dos dados do beneficiário.
    /// </summary>
    public void AtualizarBeneficiario(

        string email,
        EStatusBeneficiario novoStatus)
    {
        Status = novoStatus;
        SetUpdatedAt();
    }

}