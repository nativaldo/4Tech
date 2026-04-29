using Health.Domain.Entities;
using Health.Domain.Enums;
using Health.Domain.Shared.Abstractions.Dtos;

namespace Health.Domain.Interfaces;

public interface IBeneficiarioRepository : IRepositoryBase<Beneficiario>
{
    //Métodos específicos para Dapper (Leitura Otimizada)
    Task<(IEnumerable<BeneficiarioPageDto> Items, int TotalCount)> GetPagedAsync(
          EStatusBeneficiario Status,
          int page, int pageSize,
           CancellationToken ct);
    Task<bool> AnyWithCpfAsync(string cpf, CancellationToken ct);
    Task<Beneficiario?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<Beneficiario?> GetByCpfAsync(string cpf, CancellationToken ct);
    Task<bool> ExistsByIdAsync(Guid id, CancellationToken ct);

}