using FluentAssertions;
using Health.Domain.Entities;
using Health.Domain.Errors;
using Health.Domain.Interfaces;
using Health.Domain.Enums;
using Health.UnitTests.Application.Mothers.BeneficiarioMother;
using Moq;
using Health.Domain.Shared.Enums;
using Health.Application.UseCases.Beneficiarios.CreateBeneficiario;

namespace Health.UnitTests.Application.UseCases.Beneficiarios;

public class CreateBeneficiarioTests
{
    private readonly Mock<IBeneficiarioRepository> _beneficiarioRepoMock = new();
    private readonly Mock<IPlanoRepository> _planoRepoMock = new();
    private readonly Mock<IUnitOfWork> _uowMock = new();
    private readonly CreateBeneficiarioUseCase _sut;

    public CreateBeneficiarioTests()
    {
        _sut = new CreateBeneficiarioUseCase(
            _beneficiarioRepoMock.Object,
            _planoRepoMock.Object,
            _uowMock.Object);
    }

    [Fact]
    public async Task Execute_DeveRetornarErroConflict_QuandoCpfJaExistir()
    {
        // Arrange
        var request = BeneficiarioRequestMother.Valido();

        _beneficiarioRepoMock
            .Setup(x => x.AnyWithCpfAsync(
                request.Cpf,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await _sut.ExecuteAsync(request, default);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.ErrorType.Should().Be(ErrorType.Conflict);
        result.Error.Should().Be(ErrorRegistry.Domain.Beneficiario.CpfExistente);

        _uowMock.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Execute_DeveRetornarErroNotFound_QuandoPlanoNaoExistir()
    {
        // Arrange
        var request = BeneficiarioRequestMother.Valido();

        _beneficiarioRepoMock
            .Setup(x => x.AnyWithCpfAsync(request.Cpf, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        _planoRepoMock
            .Setup(x => x.ExistsAsync(request.PlanoId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act
        var result = await _sut.ExecuteAsync(request, default);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.ErrorType.Should().Be(ErrorType.NotFound);
        result.Error.Should().Be(ErrorRegistry.Domain.Plano.NaoEncontrado);
    }

    [Fact]
    public async Task Execute_DeveCriarBeneficiarioComSucesso_QuandoDadosForemValidos()
    {
        // Arrange
        var request = BeneficiarioRequestMother.Valido();

        _beneficiarioRepoMock
            .Setup(x => x.AnyWithCpfAsync(
                request.Cpf,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        _planoRepoMock
            .Setup(x => x.ExistsAsync(request.PlanoId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await _sut.ExecuteAsync(request, default);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();

        // Verificamos se o AddAsync foi chamado para a entidade correta
        _beneficiarioRepoMock.Verify(x => x.AddAsync(It.IsAny<Beneficiario>(), It.IsAny<CancellationToken>()), Times.Once);
        _uowMock.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}