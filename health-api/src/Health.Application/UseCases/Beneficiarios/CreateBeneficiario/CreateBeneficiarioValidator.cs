using FluentValidation;
using Health.Application.UseCases.Beneficiarios.CreateBeneficiario;
using Health.Domain.Errors;
using Health.Domain.Shared.ValueObjects;

namespace Health.Application.UseCases.Beneficiarios.Commands.CreateBeneficiario;

public sealed class CreateBeneficiarioValidator : AbstractValidator<CreateBeneficiarioRequest>
{
    public CreateBeneficiarioValidator()
    {
        // 1. Nome
        RuleFor(x => x.Nome)
            .NotEmpty()
            .WithErrorCode(ErrorRegistry.Context.Beneficiario.NomeObrigatorio.Code)
            .WithMessage(ErrorRegistry.Context.Beneficiario.NomeObrigatorio.Message);

        // 2. CPF (Delegado ao Value Object)
        RuleFor(x => x.Cpf)
            .NotEmpty()
            .WithErrorCode(ErrorRegistry.Context.Beneficiario.CpfObrigatorio.Code)
            .WithMessage(ErrorRegistry.Context.Beneficiario.CpfObrigatorio.Message)
            .Must(cpf => Cpf.Create(cpf).IsSuccess)
            .WithErrorCode(ErrorRegistry.Context.Beneficiario.CpfInvalido.Code)
            .WithMessage(ErrorRegistry.Context.Beneficiario.CpfInvalido.Message);

        // 3. Email (Delegado ao Value Object)
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithErrorCode(ErrorRegistry.Context.Beneficiario.EmailObrigatorio.Code)
            .WithMessage(ErrorRegistry.Context.Beneficiario.EmailObrigatorio.Message)
            .Must(email => Email.Create(email).IsSuccess)
            .WithErrorCode(ErrorRegistry.Context.Beneficiario.EmailInvalido.Code)
            .WithMessage(ErrorRegistry.Context.Beneficiario.EmailInvalido.Message);

        // 4. Data de Nascimento (Lógica de Maioridade)
        RuleFor(x => x.DataNascimento)
            .NotEmpty()
            .WithErrorCode(ErrorRegistry.Context.Beneficiario.DataNascimentoObrigatoria.Code)
            .WithMessage(ErrorRegistry.Context.Beneficiario.DataNascimentoObrigatoria.Message)
            .LessThan(DateTime.Today)
            .WithErrorCode(ErrorRegistry.Context.Beneficiario.DataNascimentoInvalida.Code)
            .WithMessage(ErrorRegistry.Context.Beneficiario.DataNascimentoInvalida.Message)
            .Must(BeOver18)
            .WithErrorCode(ErrorRegistry.Context.Beneficiario.MenorDeIdade.Code)
            .WithMessage(ErrorRegistry.Context.Beneficiario.MenorDeIdade.Message);

        // 5. Plano (Correção do ErrorRegistry para o campo correto)
        RuleFor(x => x.PlanoId)
            .NotEmpty()
            .WithErrorCode(ErrorRegistry.Context.Beneficiario.PlanoIdObrigatorio.Code)
            .WithMessage(ErrorRegistry.Context.Beneficiario.PlanoIdObrigatorio.Message);
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