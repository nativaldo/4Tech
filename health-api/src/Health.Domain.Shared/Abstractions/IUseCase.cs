namespace Health.Domain.Shared.Abstractions;

/// <summary>
/// Contrato mestre para todos os casos de uso do sistema.
/// </summary>
public interface IUseCase<in TRequest, TResponse>
    where TRequest : IMessage<TResponse>
    where TResponse : IUseCaseResponse
{
    Task<Result<TResponse>> ExecuteAsync(TRequest request, CancellationToken ct = default);
}