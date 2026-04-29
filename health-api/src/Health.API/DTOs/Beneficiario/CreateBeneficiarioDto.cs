namespace Health.API.DTOs.Beneficiario;

public record CreateBeneficiarioDto(
    string Nome,
    string Cpf,
    string Email,
    DateTime Nascimento,
    Guid IdPlano // O framework já valida se a string enviada é um Guid válido
);