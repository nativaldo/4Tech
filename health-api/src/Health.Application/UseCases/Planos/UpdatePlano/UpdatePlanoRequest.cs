using Health.Domain.Shared.Abstractions;

namespace Health.Application.UseCases.Planos.UpdatePlano;

public record UpdatePlanoRequest : IMessage<UpdatePlanoResponse>
{
    public Guid Id { get; private set; }
    public string Nome { get; private set; }
    public string CodigoRegistroAns { get; private set; }

    private UpdatePlanoRequest(Guid id, string nome, string codigoRegistroAns)
    {
        Id = id;
        Nome = nome;
        CodigoRegistroAns = codigoRegistroAns;
    }

    public static UpdatePlanoRequest Create(Guid id, string nome, string codigoRegistroAns)
    {
        return new UpdatePlanoRequest(id, nome, codigoRegistroAns);
    }


}

