using System;
using System.Threading;
using System.Threading.Tasks;

namespace Health.Domain.Interfaces;

/// <summary>
/// Interface para gerenciamento de unidade de trabalho e transações atômicas.
/// Suporta operações híbridas entre EF Core e Dapper via DbSession.
/// </summary>
public interface IUnitOfWork : IDisposable
{
    Task BeginTransactionAsync(CancellationToken ct = default);
    Task<bool> CommitAsync(CancellationToken ct = default);
    Task RollbackAsync(CancellationToken ct = default);


}