using FluentValidation;
using Health.Application.UseCases.Beneficiarios.UpdateBeneficiario;
using Health.Domain.Errors;
using Health.Domain.Shared.ValueObjects;

namespace Health.Application.UseCases.Beneficiarios.Commands.Create;

public sealed class UpdateBeneficiarioValidator : AbstractValidator<UpdateBeneficiarioRequest>
{
    public UpdateBeneficiarioValidator()
    {
        {
            // 1. Validação do ID
            RuleFor(x => x.Id)
                .NotEmpty()
                    .WithMessage(ErrorRegistry.Context.Beneficiario.IdObrigatorio.Message)
                    .WithErrorCode(ErrorRegistry.Context.Beneficiario.IdObrigatorio.Code);

            RuleFor(x => x.Nome)
                .NotEmpty()
                    .WithMessage(ErrorRegistry.Context.Beneficiario.NomeObrigatorio.Message)
                    .WithErrorCode(ErrorRegistry.Context.Beneficiario.NomeObrigatorio.Code)
                .MinimumLength(10)
                    .WithMessage(ErrorRegistry.Context.Beneficiario.NomeCurto.Message)
                    .WithErrorCode(ErrorRegistry.Context.Beneficiario.NomeCurto.Code);

            RuleFor(x => x.Email)
                .NotEmpty()
                    .WithMessage(ErrorRegistry.Context.Beneficiario.EmailObrigatorio.Message)
                    .WithErrorCode(ErrorRegistry.Context.Beneficiario.EmailObrigatorio.Code)
                .EmailAddress()
                    .WithMessage(ErrorRegistry.Context.Beneficiario.EmailInvalido.Message)
                    .WithErrorCode(ErrorRegistry.Context.Beneficiario.EmailInvalido.Code);

            RuleFor(x => x.DataNascimento)
                .NotEmpty()
                    .WithMessage(ErrorRegistry.Context.Beneficiario.DataNascimentoObrigatoria.Message)
                    .WithErrorCode(ErrorRegistry.Context.Beneficiario.DataNascimentoObrigatoria.Code)
                .Must(BeOver18)
                    .WithMessage(ErrorRegistry.Context.Beneficiario.MenorDeIdade.Message)
                    .WithErrorCode(ErrorRegistry.Context.Beneficiario.MenorDeIdade.Code);

            // 5. Validação do Status (Enum)
            RuleFor(x => x.Status)
                .IsInEnum()
                    .WithMessage(ErrorRegistry.Context.Beneficiario.StatusInvalido.Message)
                    .WithErrorCode(ErrorRegistry.Context.Beneficiario.StatusInvalido.Code);
        }
    }
    private bool BeOver18(DateTime dataNascimento)
    {
        var minAge = 18;
        var today = DateTime.Today;
        var age = today.Year - dataNascimento.Year;

        // Ajuste caso o aniversário ainda não tenha ocorrido este ano
        if (dataNascimento.Date > today.AddYears(-age)) age--;

        return age >= minAge;
    }

}