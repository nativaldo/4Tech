namespace Health.Domain.Shared.Abstractions;

/// <summary>
/// Define uma mensagem que retorna uma resposta específica.
/// O 'out' garante a covariância do tipo de resposta.
/// </summary>
public interface IMessage<out TResponse> where TResponse : IUseCaseResponse
{
}

/// <summary>
/// Interface base para comandos que não retornam dados (apenas Result).
/// </summary>
public interface IMessage : IMessage<None>
{
}

// Objeto para representar "Sem Conteúdo" em mensagens de comando
public record None : IUseCaseResponse;