using System.Text.RegularExpressions;
using Health.Domain.Shared.Abstractions;
using Health.Domain.Shared.Abstractions.Common.Errors;
using Health.Domain.Shared.Enums;


namespace Health.Domain.Shared.ValueObjects;

public record Cpf
{
    public const int Length = 11;
    public string Value { get; }

    private Cpf(string value) => Value = value;

    public static Result<Cpf> Create(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return Result<Cpf>.Failure(ErrorType.BadRequest, new Error("Cpf.Empty", "CPF não pode ser vazio."));

        var cleanedCpf = Regex.Replace(value, @"[^\d]", "");

        if (cleanedCpf.Length != Length)
            return Result<Cpf>.Failure(ErrorType.BadRequest, new Error("Cpf.InvalidLength", "CPF deve ter 11 dígitos."));

        if (!IsValid(cleanedCpf))
            return Result<Cpf>.Failure(ErrorType.BadRequest, new Error("Cpf.Invalid", "CPF matematicamente inválido."));

        return Result<Cpf>.Success(new Cpf(cleanedCpf));
    }

    private static bool IsValid(string cpf)
    {
        // Elimina CPFs com todos os dígitos iguais (conhecidamente inválidos)
        if (new string(cpf[0], Length) == cpf) return false;

        var tempCpf = cpf[..9];
        var sum = 0;
        int[] multiplier1 = { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
        int[] multiplier2 = { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

        for (var i = 0; i < 9; i++)
            sum += int.Parse(tempCpf[i].ToString()) * multiplier1[i];

        var remainder = sum % 11;
        remainder = remainder < 2 ? 0 : 11 - remainder;

        var digit = remainder.ToString();
        tempCpf += digit;
        sum = 0;

        for (var i = 0; i < 10; i++)
            sum += int.Parse(tempCpf[i].ToString()) * multiplier2[i];

        remainder = sum % 11;
        remainder = remainder < 2 ? 0 : 11 - remainder;
        digit += remainder;

        return cpf.EndsWith(digit);
    }

    public override string ToString() => Value;

    // Operador implícito para facilitar o uso como string
    public static implicit operator string(Cpf cpf) => cpf.Value;
}