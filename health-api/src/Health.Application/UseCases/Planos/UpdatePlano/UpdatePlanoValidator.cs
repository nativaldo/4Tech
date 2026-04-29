using FluentValidation;
using Health.Application.UseCases.Beneficiarios.UpdateBeneficiario;
using Health.Application.UseCases.Planos.UpdatePlano;
using Health.Domain.Errors;

namespace Health.Application.UseCases.Beneficiarios.Commands.Create;

public sealed class UpdatePlanoValidator : AbstractValidator<UpdatePlanoRequest>
{
    public UpdatePlanoValidator()
    {
        // 3. ID (Verifica se está vazio/zero)
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithErrorCode(ErrorRegistry.Context.Beneficiario.IdObrigatorio.Code)
            .WithMessage(ErrorRegistry.Context.Beneficiario.IdObrigatorio.Message);

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