using Health.Domain.Shared.Abstractions;

namespace Health.Application.Common;

// Toda Query/Request que precisar de paginação herdará deste record
public abstract record PagedRequest<TResponse> : IMessage<TResponse>
    where TResponse : IUseCaseResponse
{
    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 10;

    // Métodos auxiliares para facilitar o uso no Dapper/EF
    public int Skip => (Page - 1) * PageSize;
    public int Take => PageSize;
}