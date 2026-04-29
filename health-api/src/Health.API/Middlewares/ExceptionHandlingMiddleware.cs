using System.Net;
using System.Text.Json;
using Health.Domain.Shared.Abstractions;
using Health.Domain.Shared.Abstractions.Common.Errors;
using Health.Domain.Shared.Enums;
using Npgsql;

namespace Health.API.Middlewares;

public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro detetado: {Message}", ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        // Valores padrão (Erro 500)
        var statusCode = HttpStatusCode.InternalServerError;
        var errorType = ErrorType.Internal;
        var error = new Error("Server.InternalError", "Ocorreu um erro inesperado no servidor.");

        // Verificação específica para violação de Unique Key no Postgres
        // O erro pode estar na InnerException caso venha do Entity Framework
        if (exception.InnerException is PostgresException pgEx && pgEx.SqlState == "23505" ||
            exception is PostgresException directPgEx && directPgEx.SqlState == "23505")
        {
            statusCode = HttpStatusCode.Conflict; // 409 Conflict
            errorType = ErrorType.Conflict;
            error = new Error("Database.DuplicateKey", "Já existe um registro com os dados informados (conflito de duplicidade).");
        }

        context.Response.StatusCode = (int)statusCode;

        var response = Result<object>.Failure(errorType, error);

        var jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        return context.Response.WriteAsync(JsonSerializer.Serialize(response, jsonOptions));
    }
}