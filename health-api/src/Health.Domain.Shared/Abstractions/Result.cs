using Health.Domain.Shared.Abstractions.Common.Errors;
using Health.Domain.Shared.Enums;


namespace Health.Domain.Shared.Abstractions;

public class Result<TValue> : IUseCaseResponse
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public TValue? Value { get; }
    public Error Error { get; }
    public ErrorType ErrorType { get; }

    // Lista de detalhes (ex: campos inválidos e regras violadas)
    public IReadOnlyCollection<ErrorDetail> Errors { get; }

    public Result(
      TValue? value,
      bool isSuccess,
      Error error,
      ErrorType errorType,
      IEnumerable<ErrorDetail>? errors = null)
    {
        // ... seu código de validação ...
        IsSuccess = isSuccess;
        Value = value;
        Error = error;
        ErrorType = errorType;
        Errors = errors?.ToList().AsReadOnly() ?? new List<ErrorDetail>().AsReadOnly();
    }

    // Fábrica para Sucesso
    public static Result<TValue> Success(TValue value) =>
        new(value, true, Error.None, ErrorType.None);

    // Fábrica para Falhas Simples (NotFound, Conflict, Unauthorized)
    public static Result<TValue> Failure(ErrorType type, Error error) =>
        new(default, false, error, type);

    // Fábrica para Falhas de Validação (Múltiplos campos)
    public static Result<TValue> ValidationFailure(IEnumerable<ErrorDetail> errors) =>
        new(default, false, new Error("ValidationError", "Um ou mais erros de validação ocorreram."), ErrorType.Validation, errors);
}