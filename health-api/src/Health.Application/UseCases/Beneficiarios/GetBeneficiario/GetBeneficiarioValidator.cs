using FluentValidation;
using Health.Application.UseCases.Beneficiarios.GetBeneficiario;
using Health.Domain.Errors;


namespace Health.Application.UseCases.Beneficiarios.Commands.Create;

public sealed class GetBeneficiarioValidator : AbstractValidator<GetBeneficiarioRequest>
{
    public GetBeneficiarioValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage(ErrorRegistry.Domain.Beneficiario.BeneficiarioIdVazio.Message);
    }


}