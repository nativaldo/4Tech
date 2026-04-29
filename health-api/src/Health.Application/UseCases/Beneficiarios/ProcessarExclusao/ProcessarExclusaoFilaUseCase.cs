using Health.Domain.Enums;
using Health.Domain.Interfaces;
using Health.Infrastructure.Logging;

namespace Health.Application.UseCases.Beneficiarios.ProcessarExclusao;

public sealed class ProcessarExclusaoFilaUseCase(
    IExclusaoBeneficiarioRepository _queueRepository,
    IUnitOfWork _uow,
    IAppLogger _logger)
{
    public async Task ExecuteAsync(CancellationToken ct)
    {
        _logger.LogExpurgo("Iniciando fluxo de exaustão por prioridade (Critica > Alta > Normal).");

        var niveisPrioridade = Enum.GetValues<ExclusaoPrioridade>()
                                   .OrderBy(p => (int)p);

        foreach (var prioridade in niveisPrioridade)
        {
            int pValue = (int)prioridade;
            _logger.LogExpurgo($"Verificando itens de Prioridade: {prioridade} ({pValue})...");

            bool possuiItensNestaPrioridade = true;

            while (possuiItensNestaPrioridade && !ct.IsCancellationRequested)
            {
                // Busca lote usando o valor inteiro do Enum
                var itens = await _queueRepository.GetPendingByPriorityAsync(pValue, 10, ct);

                if (!itens.Any())
                {
                    possuiItensNestaPrioridade = false;
                    continue;
                }

                _logger.LogExpurgo($"Processando lote de {itens.Count()} itens - Nível {prioridade}...");

                foreach (var item in itens)
                {
                    if (ct.IsCancellationRequested) break;

                    await _uow.BeginTransactionAsync(ct);
                    try
                    {
                        await _queueRepository.HardDeleteBeneficiarioAsync(item.BeneficiarioId, ct);

                        await _queueRepository.MarkAsDeletedAsync(item.Id, ct);

                        var isSuccess = await _uow.CommitAsync(ct);
                        if (!isSuccess)
                            throw new Exception("Falha no commit da transação.");

                        _logger.LogExpurgo($"Sucesso: Beneficiário {item.BeneficiarioId} removido fisicamente.");
                    }
                    catch (Exception ex)
                    {
                        // Se der qualquer erro (FK, timeout, conexão), desfaz apenas o item atual
                        await _uow.RollbackAsync(CancellationToken.None);

                        // Loga a falha e segue para o próximo item da lista (sem estourar o Worker)
                        _logger.LogError($"Falha ao processar expurgo do beneficiário {item.BeneficiarioId}. Item ignorado nesta rodada.", ex);

                        continue;

                    }
                }

                if (itens.Count() < 10)
                    possuiItensNestaPrioridade = false;
            }
        }

        _logger.LogExpurgo("Ciclo completo de expurgo finalizado.");
    }
}