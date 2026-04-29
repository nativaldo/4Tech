using Dapper;
using Health.Domain.Entities;
using Health.Domain.Enums;
using Health.Domain.Interfaces;
using Health.Domain.Shared.Abstractions.Dtos;

namespace Health.Infrastructure.Repositories.Beneficiarios;

public class BeneficiarioRepository : RepositoryBase<Beneficiario>, IBeneficiarioRepository
{
    public BeneficiarioRepository(DbSession session) : base(session) { }

    // --- READS (DAPPER) ---   
    public async Task<(IEnumerable<BeneficiarioPageDto> Items, int TotalCount)> GetPagedAsync(
        EStatusBeneficiario status,
        int page, int pageSize,
        CancellationToken ct)
    {
        int skip = (page - 1) * pageSize;

        const string sql = @"
        SELECT 
            b.nome AS NomeCompleto, 
            b.cpf AS Cpf, 
            b.data_nascimento AS DataNascimento,
            p.nome AS Plano,
            b.status AS Status,
            COUNT(*) OVER() AS TotalCount 
        FROM hth_beneficiarios b
        INNER JOIN hth_planos p ON b.plano_id = p.id
        LEFT JOIN hth_exclusao_beneficiario_fila s ON b.id = s.beneficiario_id AND s.deletado = FALSE
        WHERE b.status = @Status 
          AND s.id IS NULL
        ORDER BY b.nome
        OFFSET @Skip LIMIT @Take";

        var parameters = new { Skip = skip, Take = pageSize, Status = (int)status };

        // Usamos _session.Dapper.Connection e passamos a transação caso o UoW tenha iniciado uma
        var items = await _session.Dapper.Connection.QueryAsync<BeneficiarioPageDto>(
            new CommandDefinition(
                sql,
                parameters,
                transaction: _session.Dapper.Transaction,
                cancellationToken: ct)
        );

        var firstItem = items.FirstOrDefault();
        int totalCount = firstItem?.TotalCount ?? 0;

        return (items, totalCount);
    }

    public async Task<bool> AnyWithCpfAsync(string cpf, CancellationToken ct)
    {
        const string sql = "SELECT EXISTS(SELECT 1 FROM hth_beneficiarios WHERE cpf = @cpf AND status = @status)";

        return await _session.Dapper.Connection.ExecuteScalarAsync<bool>(
            new CommandDefinition(
                sql,
                parameters: new
                {
                    cpf,
                    status = (int)EStatusBeneficiario.ATIVO
                },
                transaction: _session.Dapper.Transaction,
                cancellationToken: ct)
        );
    }

    // Sobrescrevendo o GetById da base para usar Dapper (opção por performance em leitura)
    public override async Task<Beneficiario?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        const string sql = "SELECT * FROM hth_beneficiarios WHERE id = @id AND status = @status";

        return await _session.Dapper.Connection.QueryFirstOrDefaultAsync<Beneficiario>(
            new CommandDefinition(
                sql,
                   parameters: new
                   {
                       id,
                       status = (int)EStatusBeneficiario.ATIVO
                   },

                transaction: _session.Dapper.Transaction,
                cancellationToken: ct)
        );
    }

    public async Task<Beneficiario?> GetByCpfAsync(string cpf, CancellationToken ct)
    {
        const string sql = @"
            SELECT * FROM hth_beneficiarios 
            WHERE cpf = @cpf 
              AND status = @status 
            LIMIT 1";

        return await _session.Dapper.Connection.QueryFirstOrDefaultAsync<Beneficiario>(
            new CommandDefinition(
                sql,
                 parameters: new
                 {
                     cpf,
                     status = (int)EStatusBeneficiario.ATIVO
                 },
                transaction: _session.Dapper.Transaction,
                cancellationToken: ct)
        );
    }
}