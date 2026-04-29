using Health.Domain.Interfaces;
using Health.Domain.Errors;
using Health.Domain.Shared.Abstractions;
using Health.Domain.Shared.Enums;
using Health.Application.UseCases.Planos.GetPlano; // Onde está o Request/Response
using Health.Domain.Shared.Abstractions.DTOs;
using Health.Application.UseCases.Beneficiarios.GetPlano;    // Para encontrar o PlanoDto

namespace Health.Application.UseCases.Planos.GetPlano; // Ajustado para a pasta correta

public sealed class GetPlanoUseCase(
    IPlanoRepository _planoRepository) : IUseCase<GetPlanoRequest, GetPlanoResponse>
{
    public async Task<Result<GetPlanoResponse>> ExecuteAsync(GetPlanoRequest request, CancellationToken ct)
    {
        var plano = await _planoRepository.GetByIdAsync(request.id, ct);

        if (plano is null)
        {
            return Result<GetPlanoResponse>.Failure(
                ErrorType.NotFound,
                ErrorRegistry.Domain.Plano.NenhumRegistroEncontrado);
        }

        var response = new GetPlanoResponse(
            Id: plano.Id,
            Nome: plano.Nome,
            CodigoRegistroAns: plano.CodigoRegistroAns
        );

        return Result<GetPlanoResponse>.Success(response);
    }
}