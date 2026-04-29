using Health.Domain.Core.Models;

namespace Health.Domain.Entities;

public sealed class ExclusaoBeneficiarioFila : Entity
{
    public Guid BeneficiarioId { get; private set; }
    public bool IsDeleted { get; private set; }

    // Regra: 1 (Critica), 2 (Alta), 3 (Normal)
    public int Prioridade { get; private set; }

    // Para desempate cronológico
    public DateTime DataSolicitacao { get; private set; }

    private ExclusaoBeneficiarioFila(Guid beneficiarioId, int prioridade, bool isDeleted = false)
    {
        BeneficiarioId = beneficiarioId;
        Prioridade = prioridade;
        DataSolicitacao = DateTime.UtcNow;
        IsDeleted = isDeleted;
    }
    public static ExclusaoBeneficiarioFila Create(Guid beneficiarioId, int prioridade)
        => new(beneficiarioId, prioridade);
}