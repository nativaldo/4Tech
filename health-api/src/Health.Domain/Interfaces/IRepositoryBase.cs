using System.Linq.Expressions;

namespace Health.Domain.Interfaces;

public interface IRepositoryBase<T> where T : class
{
    // Persistência (EF)
    Task AddAsync(T entity, CancellationToken ct);
    Task UpdateAsync(T entity, CancellationToken ct);

    // Leitura Única (EF - Necessário para Tracking de Update)
    Task<T?> GetByIdAsync(Guid id, CancellationToken ct);
    Task<bool> ExistsByIdAsync(Guid id, CancellationToken ct);
    Task Delete(T entity);
}