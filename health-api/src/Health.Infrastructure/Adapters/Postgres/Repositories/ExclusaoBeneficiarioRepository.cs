using Dapper;
using Health.Domain.Entities;
using Health.Domain.Interfaces;
using Health.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Health.Infrastructure.Adapters.Postgres.Repositories;

public sealed class ExclusaoBeneficiarioRepository : RepositoryBase<ExclusaoBeneficiarioFila>, IExclusaoBeneficiarioRepository
{
    // O construtor agora recebe a DbSession e a repassa para a base
    public ExclusaoBeneficiarioRepository(DbSession session) : base(session) { }

    public async Task<bool> ExisteParaBeneficiarioAsync(Guid beneficiarioId)
    {
        const string sql = @"
            SELECT 1 FROM hth_exclusao_beneficiario_fila 
            WHERE beneficiario_id = @Id 
              AND deletado = FALSE 
            LIMIT 1";

        // Acesso via _session.Dapper para garantir conexão aberta e transação do UoW
        var result = await _session.Dapper.Connection.ExecuteScalarAsync<int?>(
            sql,
            new { Id = beneficiarioId },
            _session.Dapper.Transaction // Passar explicitamente a transação é vital para o Dapper
        );

        return result.HasValue;
    }

    public async Task<IEnumerable<ExclusaoBeneficiarioFila>> GetPendingByPriorityAsync(int limit, int prioridade, CancellationToken ct)
    {
        const string SqlGetPendingItems = @"
            SELECT 
                q.id AS Id, 
                q.beneficiario_id AS BeneficiarioId, 
                b.nome AS NomeBeneficiario
            FROM hth_exclusao_beneficiario_fila q
            INNER JOIN hth_beneficiarios b ON q.beneficiario_id = b.id
            WHERE q.deletado = false
              AND q.prioridade = @Prioridade
            ORDER BY q.data_solicitacao ASC
            LIMIT @Limit";

        return await _session.Dapper.Connection.QueryAsync<ExclusaoBeneficiarioFila>(new CommandDefinition(
            SqlGetPendingItems,
            new { Limit = limit, Prioridade = prioridade },
            transaction: _session.Dapper.Transaction,
            cancellationToken: ct));
    }

    // --- ESCRITA (ENTITY FRAMEWORK VIA BASE) ---
    public async Task AdicionarFilaAsync(ExclusaoBeneficiarioFila solicitacaoExclusao, CancellationToken ct)
    {
        // Usa o método AddAsync da RepositoryBase que utiliza o _session.Context
        await AddAsync(solicitacaoExclusao, ct);
    }

    // 2. Exclusão Física (Dapper)
    public async Task HardDeleteBeneficiarioAsync(Guid beneficiarioId, CancellationToken ct)
    {
        const string sql = "DELETE FROM hth_beneficiarios WHERE id = @beneficiarioId";

        await _session.Dapper.Connection.ExecuteAsync(
            new CommandDefinition(
                sql,
                new { beneficiarioId },
                transaction: _session.Dapper.Transaction,
                cancellationToken: ct)
        );
    }

    // 3. Marcação de Fila (Dapper)
    public async Task MarkAsDeletedAsync(Guid id, CancellationToken ct)
    {
        const string sql = @"
            UPDATE hth_exclusao_beneficiario_fila 
            SET deletado = true, 
                processed_at = NOW() 
            WHERE id = @id";

        await _session.Dapper.Connection.ExecuteAsync(
            new CommandDefinition(
                sql,
                new { id },
                transaction: _session.Dapper.Transaction,
                cancellationToken: ct)
        );
    }
}