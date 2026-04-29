using Health.Domain.Interfaces;
using Health.Infrastructure.Adapters.Postgres;
using Microsoft.EntityFrameworkCore.Storage;

namespace Health.Infrastructure.Persistence;

public sealed class PostgresUnitOfWork(AppDbContext _context) : IUnitOfWork
{
    private IDbContextTransaction? _efTransaction;

    public async Task BeginTransactionAsync(CancellationToken ct = default)
    {
        // Se a transação já existe, não faz nada (Idempotência)
        if (_efTransaction is not null) return;

        // O EF Core inicia a transação. 
        // A DapperSession (via DbSession) refletirá isso automaticamente.
        _efTransaction = await _context.Database.BeginTransactionAsync(ct);
    }

    public async Task<bool> CommitAsync(CancellationToken ct = default)
    {
        try
        {
            // 1. Salva as alterações rastreadas pelo EF
            await _context.SaveChangesAsync(ct);

            // 2. Comita a transação se ela foi aberta explicitamente
            if (_efTransaction is not null)
            {
                await _efTransaction.CommitAsync(ct);
            }

            return true;
        }
        catch
        {
            await RollbackAsync(ct);
            throw;
        }
        finally
        {
            ResetTransaction();
        }
    }

    public async Task RollbackAsync(CancellationToken ct = default)
    {
        if (_efTransaction is not null)
        {
            await _efTransaction.RollbackAsync(ct);
        }
        ResetTransaction();
    }

    private void ResetTransaction()
    {
        _efTransaction?.Dispose();
        _efTransaction = null;
    }

    public void Dispose()
    {
        _efTransaction?.Dispose();
        _efTransaction = null;
        GC.SuppressFinalize(this);
    }
}