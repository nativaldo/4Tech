using FluentAssertions;
using Health.Application.UseCases.Beneficiarios.GetBeneficiario;
using Health.Domain.Errors;
using Health.Domain.Interfaces;
using Health.Domain.Shared.Enums;
using Health.UnitTests.Application.Mothers.BeneficiarioMother;
using Health.UnitTests.Domain.Mothers;
using Moq;
using Xunit;

namespace Health.UnitTests.Application.UseCases.Beneficiarios;

public class GetBeneficiarioTests
{
    private readonly Mock<IBeneficiarioRepository> _repoMock = new();
    private readonly GetBeneficiarioCase _sut;

    public GetBeneficiarioTests()
    {
        _sut = new GetBeneficiarioCase(_repoMock.Object);
    }

    [Fact]
    public async Task Execute_DeveRetornarErroNotFound_QuandoBeneficiarioNaoExistir()
    {
        // Arrange
        var request = GetBeneficiarioRequestMother.IdAleatorio();

        _repoMock.Setup(x => x.GetByIdAsync(request.Id, It.IsAny<CancellationToken>()))
                 .ReturnsAsync((Health.Domain.Entities.Beneficiario?)null);

        // Act
        var result = await _sut.ExecuteAsync(request, default);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.ErrorType.Should().Be(ErrorType.NotFound);
        result.Error.Should().Be(ErrorRegistry.Domain.Beneficiario.NaoEncontrado);
    }

    [Fact]
    public async Task Execute_DeveRetornarResponse_QuandoBeneficiarioForEncontrado()
    {
        // Arrange
        var beneficiarioExistente = BeneficiarioMother.Valido();
        var request = GetBeneficiarioRequestMother.ComId(beneficiarioExistente.Id);

        _repoMock.Setup(x => x.GetByIdAsync(request.Id, It.IsAny<CancellationToken>()))
                 .ReturnsAsync(beneficiarioExistente);

        // Act
        var result = await _sut.ExecuteAsync(request, default);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.Nome.Should().Be(beneficiarioExistente.NomeCompleto);
        result.Value.Cpf.Should().Be(beneficiarioExistente.Cpf);

        _repoMock.Verify(x => x.GetByIdAsync(request.Id, It.IsAny<CancellationToken>()), Times.Once);
    }
}