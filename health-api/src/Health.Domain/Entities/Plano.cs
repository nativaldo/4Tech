using Health.Domain.Core.Models;

namespace Health.Domain.Entities;

public sealed class Plano : Entity
{
    public string Nome { get; private set; }
    public string CodigoRegistroAns { get; private set; }

    // Construtor privado para forçar o uso do Factory Method
    private Plano(string nome, string codigoRegistroAns)
    {
        Nome = nome;
        CodigoRegistroAns = codigoRegistroAns;
    }

    // Construtor necessário para o EF Core
    private Plano() { }

    // Factory Method: Única forma de criar um plano no sistema
    public static Plano Create(string nome, string codigoRegistroAns)
    {
        return new Plano(nome, codigoRegistroAns);
    }

    public void UpdateEntity(string nome, string codigoRegistroAns)
    {
        Nome = nome;
        CodigoRegistroAns = codigoRegistroAns;
        SetUpdatedAt();
    }
}