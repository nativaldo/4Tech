using Microsoft.AspNetCore.Mvc;
using Health.Domain.Shared.Abstractions;
using Health.Domain.Shared.Enums;
using Health.Domain.Core.Interfaces;

namespace Health.Api.v1.Controllers;

[ApiController]
public abstract class BaseController : ControllerBase
{
    private IMediator? _mediator;

    /// <summary>
    /// Lazy loading do Mediator Próprio via Service Locator.
    /// </summary>
    protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<IMediator>();

    /// <summary>
    /// Processa o Result e garante o contrato de resposta padrão.
    /// </summary>
    protected IActionResult ProcessResult<T>(Result<T> result)
    {
        if (result.IsSuccess)
        {
            if (result.Value is None || result.Value == null)
                return NoContent();

            return Ok(result.Value);
        }

        return MapError(result);
    }

    /// <summary>
    /// Constrói o objeto de erro estruturado conforme o padrão Master.
    /// </summary>
    private IActionResult MapError<T>(Result<T> result)
    {
        // 1. Estruturação do corpo da resposta (Contrato Estrito)
        var errorResponse = new
        {
            error = result.ErrorType.ToString(), // Ex: "ValidationError"
            message = result.Error.Message,     // Ex: "CPF inválido"
            details = result.Errors?.Select(e => new
            {
                field = e.Field.ToLowerInvariant(),
                rule = e.Rule.ToLowerInvariant()
            }) ?? Enumerable.Empty<object>()
        };

        // 2. Mapeamento para o Status Code HTTP correto
        return result.ErrorType switch
        {
            ErrorType.Validation => UnprocessableEntity(errorResponse), // 422
            ErrorType.NotFound => NotFound(errorResponse),              // 404
            ErrorType.Conflict => Conflict(errorResponse),              // 409
            ErrorType.Unauthorized => Unauthorized(errorResponse),      // 401       
            ErrorType.Internal => StatusCode(StatusCodes.Status500InternalServerError, new
            {
                error = "InternalServerError",
                message = "Ocorreu um erro inesperado.",
                details = new[] { new { field = "server", rule = "critical" } }
            })
        };
    }
}