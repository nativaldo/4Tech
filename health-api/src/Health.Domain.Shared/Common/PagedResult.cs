namespace Health.Domain.Shared.Abstractions.Common;

public record PagedResult<T> : IUseCaseResponse
{
    public IEnumerable<T> Items { get; init; }
    public int CurrentPage { get; init; }
    public int PageSize { get; init; }
    public int TotalItems { get; init; }
    public int TotalPages => (int)Math.Ceiling(TotalItems / (double)PageSize);

    private PagedResult(IEnumerable<T> items, int page, int pageSize, int total)
    {
        Items = items;
        CurrentPage = page;
        PageSize = pageSize;
        TotalItems = total;
    }

    // O Factory Method permite que você converta qualquer lista em PagedResult
    public static PagedResult<T> Create(IEnumerable<T> items, int page, int pageSize, int total)
        => new(items, page, pageSize, total);
}