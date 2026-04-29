using System.Runtime.InteropServices;
using Health.Domain.Entities;
using Health.Domain.Shared.Abstractions.DTOs;
using Microsoft.VisualBasic.FileIO;

namespace Health.Domain.Interfaces;

public interface IPlanoRepository : IRepositoryBase<Plano>
{
    Task<PlanoDto> GetPlanoByIdAsync(Guid id, CancellationToken ct = default);
    Task<bool> ExistsAsync(Guid id, CancellationToken ct);
    Task<IEnumerable<PlanoDto>> GetAllAsync(CancellationToken ct);
    Task<bool> ExistsByNomeAsync(string nome, CancellationToken ct);
    Task<bool> ExistsByCodigoAnsAsync(string codigoAns, CancellationToken ct);
    Task Delete(Plano plano);

}