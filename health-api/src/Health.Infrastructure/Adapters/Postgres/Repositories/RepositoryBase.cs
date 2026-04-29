using Health.Domain.Core.Models;
using Health.Domain.Interfaces;
using Health.Infrastructure.Adapters.Postgres;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Health.Infrastructure.Repositories;

public abstract class RepositoryBase<TEntity>(DbSession session) : IRepositoryBase<TEntity>
    where TEntity : Entity
{
    protected readonly DbSession _session = session;

    // Propriedade para facilitar o acesso ao Context e aos DbSets específicos
    protected AppDbContext Context => _session.Context;
    protected DbSet<TEntity> Entities => _session.Context.Set<TEntity>();

    // 1. Escrita: Adicionar
    public virtual async Task AddAsync(TEntity entity, CancellationToken ct)
    {
        await Entities.AddAsync(entity, ct);
    }

    // 2. Escrita: Update (EF Core não possui UpdateAsync nativo, pois é uma operação de estado)
    public virtual Task UpdateAsync(TEntity entity, CancellationToken ct)
    {
        Entities.Update(entity);
        return Task.CompletedTask;
    }

    // 3. Leitura Única com Tracking (Essencial para o ciclo de vida do EF)
    public virtual async Task<TEntity?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        return await Entities.FirstOrDefaultAsync(e => e.Id == id, ct);
    }

    // 4. Verificação de Existência (Leitura rápida)
    public virtual async Task<bool> ExistsByIdAsync(Guid id, CancellationToken ct)
    {
        return await Entities.AnyAsync(e => e.Id == id, ct);
    }

    // 5. Delete (Marcação de remoção no Contexto)
    public virtual Task Delete(TEntity entity)
    {
        Entities.Remove(entity);
        return Task.CompletedTask;
    }
}