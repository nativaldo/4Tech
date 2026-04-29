using Health.Domain.Shared.Abstractions;
using System.Linq;

namespace Health.Application.UseCases.Beneficiarios.CreatePlano;

// Removemos os parâmetros da assinatura da classe para usar o construtor explícito
public record CreatePlanoRequest(
    string Nome,
    string CodigoRegistroAns
) : IMessage<CreatePlanoResponse>
{
    public static CreatePlanoRequest Create(string nome, string codigoAns)
    {
        return new CreatePlanoRequest(
            Nome: nome?.Trim() ?? string.Empty,
            CodigoRegistroAns: codigoAns?.Trim() ?? string.Empty
        );
    }
}