using Health.Domain.Shared.Abstractions;

namespace Health.Application.UseCases.Planos.DeletePlano;

public record DeletePlanoRequest : IMessage<DeletePlanoResponse>
{
    public Guid Id { get; private set; }
    public string Nome { get; private set; }
    public string CodigoRegistroAns { get; private set; }

    private DeletePlanoRequest(Guid id, string nome, string codigoRegistroAns)
    {
        Id = id;
        Nome = nome;
        CodigoRegistroAns = codigoRegistroAns;
    }

    public DeletePlanoRequest Create(Guid id, string nome, string codigoRegistroAns)
    {
        return new DeletePlanoRequest(id, nome, codigoRegistroAns);
    }


}

