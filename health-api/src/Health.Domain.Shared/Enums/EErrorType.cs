namespace Health.Domain.Shared.Enums;// Ajuste para o seu namespace

public enum ErrorType
{
    None = 0,
    Validation = 422,
    NotFound = 404,
    Conflict = 409,
    Internal = 500,
    BadRequest = 400,
    Unauthorized = 501,
    Forbidden = 502
}