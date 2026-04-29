namespace Health.Domain.Shared.Abstractions;

/// <summary>
/// Interface de marcação para unificar o TResponse dos UseCases.
/// Permite que o Mediator e o Result Pattern tratem retornos simples e paginados de forma polimórfica.
/// </summary>
public interface IUseCaseResponse { }