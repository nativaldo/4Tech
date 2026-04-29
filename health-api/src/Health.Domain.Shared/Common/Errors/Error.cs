namespace Health.Domain.Shared.Abstractions.Common.Errors;

// Record para o erro principal
public record Error(string Code, string Message)
{
    public static readonly Error None = new(string.Empty, string.Empty);
}