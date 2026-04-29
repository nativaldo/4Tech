using Health.API.DTOs.Beneficiario;
using Health.Application.UseCases.Beneficiarios.CreateBeneficiario;
using Health.Application.UseCases.Beneficiarios.GetListBeneficiarios;
using Health.Application.UseCases.Beneficiarios.SolicitacaoDeleteBeneficiario;
using Health.Application.UseCases.Beneficiarios.UpdateBeneficiario;
using Health.Domain.Shared.Abstractions.Common.Errors;
using Microsoft.AspNetCore.Mvc;

namespace Health.Api.v1.Controllers;

[ApiController]
[Route("api/beneficiarios")]
public class BeneficiariosController : BaseController
{
    /// <summary>
    /// Lista de beneficiários com paginação (Leitura via Dapper no Handler).
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<GetListBeneficiariosResponse>), 200)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> List([FromQuery] GetBeneficiariosDto dto, CancellationToken ct = default)
    {
        var query = GetListBeneficiarioRequest.Create(
            dto.Page,
            dto.PageSize
            );

        var result = await Mediator.SendAsync(query, ct);
        return ProcessResult(result);
    }

    /// <summary>
    /// Formulário para criar um novo beneficiário (Cadastro).
    /// </summary>
    [HttpPost("novo")]
    [ProducesResponseType(typeof(CreateBeneficiarioResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Create([FromBody] CreateBeneficiarioDto dto, CancellationToken ct)
    {
        var request = CreateBeneficiarioRequest.Create(
            dto.Nome, dto.Cpf, dto.Email, dto.Nascimento, dto.IdPlano);

        var result = await Mediator.SendAsync(request, ct);
        return ProcessResult(result);
    }

    /// <summary>
    /// Edita um beneficiário existente.
    /// </summary>
    [HttpPut("editar/{id:guid}")]
    [ProducesResponseType(typeof(UpdateBeneficiarioResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateBeneficiarioDto dto, CancellationToken ct)
    {
        var request = UpdateBeneficiarioRequest.Create(
            id,
            dto.NomeCompleto,
            dto.Email,
            dto.DataNascimento,
            dto.Status
            );

        var result = await Mediator.SendAsync(request, ct);
        return ProcessResult(result);
    }

    /// <summary>
    /// Faz a solicitação para excluir um beneficiário existente (insere na fila de expurgo).
    /// </summary>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(typeof(SolicitacaoDeleteBeneficiarioResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Delete([FromRoute] Guid id, [FromQuery] int prioridade, CancellationToken ct)
    {
        // Criamos o comando de exclusão apenas com o ID da rota
        // O Request/Command de exclusão deve ser específico para esta intenção
        var request = new SolicitacaoDeleteBeneficiarioRequest(id, prioridade);

        var result = await Mediator.SendAsync(request, ct);

        // ProcessResult deve retornar NoContent (204) em caso de sucesso
        return ProcessResult(result);
    }
}