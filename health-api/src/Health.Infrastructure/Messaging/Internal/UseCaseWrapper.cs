using Health.Domain.Shared.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Health.Infrastructure.Messaging.Internal;

internal abstract class UseCaseWrapper<TResponse>
    where TResponse : IUseCaseResponse
{
    public abstract Task<Result<TResponse>> HandleAsync(object message, IServiceProvider sp, CancellationToken ct);
}
internal class UseCaseWrapperImpl<TRequest, TResponse> : UseCaseWrapper<TResponse>
    where TRequest : IMessage<TResponse>
    where TResponse : IUseCaseResponse
{
    public override async Task<Result<TResponse>> HandleAsync(object message, IServiceProvider sp, CancellationToken ct)
    {
        // Obtém o UseCase do container de DI
        var useCase = sp.GetRequiredService<IUseCase<TRequest, TResponse>>();

        // Executa o UseCase fazendo o cast da mensagem para o tipo correto
        return await useCase.ExecuteAsync((TRequest)message, ct);
    }
}