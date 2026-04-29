
using Health.Domain.Shared.Abstractions;

namespace Health.Domain.Core.Interfaces;
/// <summary>
/// Define o contrato para despacho de mensagens.
/// </summary>
public interface IMediator
{
    // Adicione a restrição no final da assinatura do método
    Task<Result<TResponse>> SendAsync<TResponse>(
        IMessage<TResponse> message,
        CancellationToken ct = default)
        where TResponse : IUseCaseResponse;
}

