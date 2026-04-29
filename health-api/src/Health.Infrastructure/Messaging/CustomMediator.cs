using System.Collections.Concurrent;
using Health.Domain.Shared.Abstractions;
using Health.Infrastructure.Messaging.Internal;
using Health.Domain.Core.Interfaces;

namespace Health.Infrastructure.Messaging;

public class CustomMediator : IMediator
{
    // 1. Declare o campo privado
    private readonly IServiceProvider _serviceProvider;
    private static readonly ConcurrentDictionary<Type, object> _wrappers = new();

    // 2. Injete o IServiceProvider no construtor
    public CustomMediator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task<Result<TResponse>> SendAsync<TResponse>(
        IMessage<TResponse> message,
        CancellationToken ct = default)
        where TResponse : IUseCaseResponse
    {
        var messageType = message.GetType();

        var wrapper = _wrappers.GetOrAdd(messageType, t =>
        {
            var wrapperType = typeof(UseCaseWrapperImpl<,>).MakeGenericType(t, typeof(TResponse));
            return Activator.CreateInstance(wrapperType)!;
        });

        // 3. Agora o _serviceProvider existe e pode ser usado aqui
        return await ((UseCaseWrapper<TResponse>)wrapper).HandleAsync(message, _serviceProvider, ct);
    }
}