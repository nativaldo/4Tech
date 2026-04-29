namespace Health.Domain.Exceptions;

/// <summary>
/// Classe base para todas as exceções de negócio (Regras de Domínio).
/// </summary>
public abstract class DomainException : Exception
{
    public string ErrorCode { get; }

    protected DomainException(string message, string errorCode)
        : base(message)
    {
        ErrorCode = errorCode;
    }
}