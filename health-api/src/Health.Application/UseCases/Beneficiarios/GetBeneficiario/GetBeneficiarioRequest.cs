using Health.Domain.Enums;
using Health.Domain.Shared.Abstractions;

namespace Health.Application.UseCases.Beneficiarios.GetBeneficiario;

public record GetBeneficiarioRequest(Guid Id) : IMessage<GetBeneficiarioResponse>;

