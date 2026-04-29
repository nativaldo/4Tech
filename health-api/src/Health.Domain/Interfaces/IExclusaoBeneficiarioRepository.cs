using Health.Domain.Entities;
namespace Health.Domain.Interfaces;

public interface IExclusaoBeneficiarioRepository : IRepositoryBase<ExclusaoBeneficiarioFila>
{
    // Escrita/Leitura simples (EF)
    Task AdicionarFilaAsync(ExclusaoBeneficiarioFila exclusao, CancellationToken ct);
    Task HardDeleteBeneficiarioAsync(Guid beneficiarioId, CancellationToken ct);
    // Leitura (Dapper + Postgres)
    Task<bool> ExisteParaBeneficiarioAsync(Guid beneficiarioId);
    Task<IEnumerable<ExclusaoBeneficiarioFila>> GetPendingByPriorityAsync(int limit, int prioridade, CancellationToken ct);
    Task MarkAsDeletedAsync(Guid id, CancellationToken ct);
}
