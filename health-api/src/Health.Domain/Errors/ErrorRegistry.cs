using Health.Domain.Shared.Abstractions;
using Health.Domain.Shared.Abstractions.Common.Errors; // Integração com o contrato global

namespace Health.Domain.Errors;

public static class ErrorRegistry
{
    /// <summary>
    /// Erros de Contexto: Validados na borda da aplicação (Mediator/Validators).
    /// Referem-se à sintaxe e contrato dos dados enviados.
    /// </summary>
    public static class Context
    {
        public static class Beneficiario
        {
            public static readonly Error NomeObrigatorio = new("Context.Beneficiario.Nome", "O nome completo deve ser informado.");
            public static readonly Error CpfObrigatorio = new("Context.Beneficiario.CpfReq", "O CPF é um campo obrigatório.");
            public static readonly Error CpfInvalido = new("Context.Beneficiario.CpfInv", "O CPF informado não é válido ou está em formato incorreto.");
            public static readonly Error EmailObrigatorio = new("Context.Beneficiario.EmailObrigatorio", "O e-mail é um campo obrigatório.");
            public static readonly Error EmailInvalido = new("Context.Beneficiario.EmailInvalido", "O e-mail informado não é válido ou está em formato incorreto.");
            public static readonly Error DataNascimentoInvalida = new("Context.Beneficiario.Data", "A data de nascimento é inválida.");
            public static readonly Error DataNascimentoObrigatoria = new("Context.Beneficiario.Data", "A data de nascimento é obrigatória.");
            public static readonly Error PlanoIdObrigatorio = new("Context.Beneficiario.PlanoId", "O identificador do plano deve ser fornecido.");
            public static readonly Error IdObrigatorio = new("Context.Beneficiario.Id", "O identificador do beneficiário é obrigatório.");
            public static readonly Error StatusInvalido = new("Context.Beneficiario.Status", "O status informado não é válido.");
            public static readonly Error NomeCurto = new("Context.Beneficiario.NomeCurto", "O nome completo deve conter ao menos 10 caracteres.");
            public static readonly Error MenorDeIdade = new("Context.Beneficiario.MenorDeIdade", "O beneficiário deve possuir ao menos 18 anos.");

        }

        public static class Plano
        {
            public static readonly Error NomeObrigatorio = new("Context.Plano.Nome", "O nome do plano é obrigatório.");
            public static readonly Error RegistroAnsInvalido = new("Context.Plano.Ans", "O código de registro ANS é obrigatório.");
            public static readonly Error CodigoAnsObrigatorio = new("Context.Plano.CodigoAns", "O código de registro ANS é obrigatório.");
            public static readonly Error MaxLengthCodigoAns = new("Context.Plano.CodigoAns", "O código de registro ANS deve ter no máximo 50 caracteres.");
            public static readonly Error MaxLengthNome = new("Context.Plano.Nome", "O nome do plano deve ter no máximo 150 caracteres.");
        }
    }

    /// <summary>
    /// Erros de Domínio: Validados no UseCase ou Entidades.
    /// Referem-se ao estado do negócio e regras de integridade.
    /// </summary>
    public static class Domain
    {
        public static class Beneficiario
        {
            public static readonly Error CpfExistente = new("Domain.Beneficiario.CpfDuplicate", "Já existe um beneficiário cadastrado com este CPF.");
            public static readonly Error NaoEncontrado = new("Domain.Beneficiario.NotFound", "Beneficiário não localizado.");
            public static readonly Error ExisteNaFila = new("Domain.Beneficiario.JaEmProcessoExclusao", "Já existe uma solicitação de exclusão em andamento para este beneficiário.");
            public static readonly Error BeneficiarioIdVazio = new("Domain.Beneficiario.IdVazio", "O identificador do beneficiário é obrigatório.");
        }

        public static class Plano
        {
            public static readonly Error NaoEncontrado = new("Domain.Plano.NotFound", "O plano informado não existe no sistema.");
            public static readonly Error NomeDuplicado = new("Domain.Plano.NomeDuplicate", "Já existe um plano com este nome.");
            public static readonly Error CodigoAnsDuplicado = new("Domain.Plano.CodigoAnsDuplicate", "Já existe um plano com este código ANS.");
            public static readonly Error NenhumRegistroEncontrado = new("Domain.Plano.RetornoSemRegistros", "Nenhum registro encontrado.");
        }

        public static class Common
        {
            public static readonly Error CommitError = new("Domain.Common.Persistence", "Ocorreu um erro ao persistir os dados no banco.");
        }
    }
}