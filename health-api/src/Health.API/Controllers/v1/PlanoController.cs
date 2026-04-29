
using Health.Api.v1.Controllers;
using Health.API.DTOs.Plano;
using Health.API.v1.DTOs.Plano;
using Health.Application.UseCases.Beneficiarios.CreatePlano;
using Health.Application.UseCases.Beneficiarios.GetPlano;
using Health.Application.UseCases.Planos.GetPlano;
using Health.Application.UseCases.Planos.UpdatePlano;
using Health.Domain.Shared.Abstractions.Common.Errors;
using Microsoft.AspNetCore.Mvc;

namespace Health.API.Controllers.v1;

[ApiController]
[Route("api/planos")]
public class PlanoController : BaseController
{
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<GetPlanoResponse>), 200)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> List(CancellationToken ct = default)
    {
        var query = new GetListPlanosRequest();

        var result = await Mediator.SendAsync(query, ct);
        return ProcessResult(result);
    }

    /// <summary>
    /// Formulário para criar um novo beneficiário (Cadastro).
    /// </summary>
    [HttpPost("novo")]
    [ProducesResponseType(typeof(CreatePlanoResponse), 200)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Create([FromBody] CreatePlanoDto dto, CancellationToken ct)
    {
        var request = CreatePlanoRequest.Create(
            dto.Nome, dto.CodigoRegistroAns);

        var result = await Mediator.SendAsync(request, ct);
        return ProcessResult(result);
    }

    /// <summary>
    /// Formulário para editar um beneficiário existente.
    /// </summary>
    [HttpPut("editar/{id:guid}")]
    [ProducesResponseType(typeof(CreatePlanoResponse), 200)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdatePlanoDto dto, CancellationToken ct)
    {
        var request = UpdatePlanoRequest.Create(
            id,
            dto.Nome,
            dto.CodigoRegistroAns
            );

        var result = await Mediator.SendAsync(request, ct);
        return ProcessResult(result);
    }
}