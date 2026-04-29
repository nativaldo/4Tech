using System.Text.RegularExpressions;
using Health.Domain.Shared.Abstractions;
using Health.Domain.Shared.Abstractions.Common.Errors;
using Health.Domain.Shared.Enums;

namespace Health.Domain.Shared.ValueObjects;

public record Email
{
    private static readonly Regex EmailRegex = new(
        @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
        RegexOptions.Compiled | RegexOptions.IgnoreCase);

    public string Value { get; }

    private Email(string value) => Value = value;

    public static Result<Email> Create(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return Result<Email>.Failure(ErrorType.Validation, new Error("Email.Empty", "O e-mail deve ser informado."));

        var trimmedEmail = value.Trim();

        if (trimmedEmail.Length > 255)
            return Result<Email>.Failure(ErrorType.Validation, new Error("Email.TooLong", "O e-mail é muito longo."));

        if (!EmailRegex.IsMatch(trimmedEmail))
            return Result<Email>.Failure(ErrorType.Validation, new Error("Email.InvalidFormat", "O formato do e-mail é inválido."));

        return Result<Email>.Success(new Email(trimmedEmail));
    }

    public override string ToString() => Value;

    public static implicit operator string(Email email) => email.Value;
}