using Dapper;
using Health.Domain.Entities;
using Health.Domain.Interfaces;
using Health.Domain.Shared.Abstractions.DTOs;
using Health.Infrastructure.Repositories;

namespace Health.Infrastructure.Adapters.Postgres.Repositories;

public sealed class PlanoRepository : RepositoryBase<Plano>, IPlanoRepository
{
    // Construtor ajustado para receber a DbSession e repassar para a base
    public PlanoRepository(DbSession session) : base(session) { }

    /// <summary>
    /// Verifica existência usando Dapper para evitar overhead de tracking do EF.
    /// </summary>
    public async Task<bool> ExistsAsync(Guid id, CancellationToken ct)
    {
        const string sql = "SELECT EXISTS(SELECT 1 FROM hth_planos WHERE id = @id)";

        return await _session.Dapper.Connection.ExecuteScalarAsync<bool>(
            new CommandDefinition(
                sql,
                new { id },
                transaction: _session.Dapper.Transaction,
                cancellationToken: ct)
        );
    }

    /// <summary>
    /// Retorna todos os planos usando Dapper. 
    /// </summary>
    public async Task<IEnumerable<PlanoDto>> GetAllAsync(CancellationToken ct)
    {
        const string sql = "SELECT id, nome, codigo_registro_ans as CodigoRegistroAns FROM hth_planos";

        return await _session.Dapper.Connection.QueryAsync<PlanoDto>(
            new CommandDefinition(
                sql,
                transaction: _session.Dapper.Transaction,
                cancellationToken: ct)
        );
    }

    public async Task<bool> ExistsByNomeAsync(string nome, CancellationToken ct)
    {
        const string sql = "SELECT EXISTS(SELECT 1 FROM hth_planos WHERE nome = @nome)";

        return await _session.Dapper.Connection.ExecuteScalarAsync<bool>(
            new CommandDefinition(
                sql,
                new { nome },
                transaction: _session.Dapper.Transaction,
                cancellationToken: ct)
        );
    }

    public async Task<bool> ExistsByCodigoAnsAsync(string codigoAns, CancellationToken ct)
    {
        const string sql = "SELECT EXISTS(SELECT 1 FROM hth_planos WHERE codigo_registro_ans = @codigoAns)";

        return await _session.Dapper.Connection.ExecuteScalarAsync<bool>(
            new CommandDefinition(
                sql,
                new { codigoAns },
                transaction: _session.Dapper.Transaction,
                cancellationToken: ct)
        );
    }

    public async Task<PlanoDto?> GetPlanoByIdAsync(Guid id, CancellationToken ct)
    {
        const string sql = @"
        SELECT 
            id AS Id, 
            nome AS Nome, 
            codigo_registro_ans AS CodigoRegistroAns 
        FROM hth_planos 
        WHERE id = @id 
        LIMIT 1";

        return await _session.Dapper.Connection.QueryFirstOrDefaultAsync<PlanoDto>(
            new CommandDefinition(
                sql,
                new { id },
                transaction: _session.Dapper.Transaction,
                cancellationToken: ct)
        );
    }
}