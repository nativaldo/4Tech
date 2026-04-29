using FluentValidation;
using Health.Domain.Errors;

namespace Health.Application.UseCases.Beneficiarios.CreatePlano;

public sealed class CreatePlanoValidator : AbstractValidator<CreatePlanoRequest>
{
    public CreatePlanoValidator()
    {
        // 1. Validação do Nome
        RuleFor(x => x.Nome)
            .NotEmpty()
                .WithErrorCode(ErrorRegistry.Context.Plano.NomeObrigatorio.Code)
                .WithMessage(ErrorRegistry.Context.Plano.NomeObrigatorio.Message)
            .MaximumLength(150)
                .WithMessage(ErrorRegistry.Context.Plano.MaxLengthNome.Message);

        // 2. Validação do Código ANS (Formato Livre)
        RuleFor(x => x.CodigoRegistroAns)
            .NotEmpty()
                .WithErrorCode(ErrorRegistry.Context.Plano.CodigoAnsObrigatorio.Code)
                .WithMessage(ErrorRegistry.Context.Plano.CodigoAnsObrigatorio.Message)
            .MaximumLength(50)
                .WithMessage(ErrorRegistry.Context.Plano.MaxLengthCodigoAns.Message);
    }
}