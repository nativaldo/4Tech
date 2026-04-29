using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Health.Domain.Enums;

namespace Health.API.DTOs.Beneficiario;

//[SwaggerSchema(Title = "Update de Beneficiário", Description = "Contrato para atualização de dados permitidos.")]
public record UpdateBeneficiarioDto
{
    public string NomeCompleto { get; init; } = string.Empty;
    public string? Email { get; init; } = string.Empty;
    public required DateTime DataNascimento { get; init; }

    [EnumDataType(typeof(EStatusBeneficiario))]
    [Description("1 = ATIVO, 2 = INATIVO")]
    public EStatusBeneficiario Status { get; init; }
}