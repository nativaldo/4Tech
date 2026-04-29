using FluentAssertions;
using Health.Application.UseCases.Beneficiarios.GetList;
using Health.Domain.Enums;
using Health.Domain.Interfaces;
using Health.Domain.Shared.Abstractions.Dtos;
using Health.UnitTests.Application.Mothers.BeneficiarioMother;
using Health.UnitTests.Domain.Mothers;
using Moq;


namespace Health.UnitTests.Application.UseCases.Beneficiarios;

public class GetListBeneficiarioTests
{
    private readonly Mock<IBeneficiarioRepository> _repoMock = new();
    private readonly GetListBeneficiarioUseCase _sut;

    public GetListBeneficiarioTests()
    {
        _sut = new GetListBeneficiarioUseCase(_repoMock.Object);
    }

    [Fact]
    public async Task Execute_DeveRetornarListaPaginada_QuandoExistiremDados()
    {
        // Arrange
        var request = GetListBeneficiarioRequestMother.Padrao();
        var (items, totalCount) = BeneficiarioDtoMother.PaginaComUmItem();

        _repoMock.Setup(x => x.GetPagedAsync(request.Status, It.IsAny<int>(), It.IsAny<int>(), default))
                 .ReturnsAsync((items, totalCount));

        // Act
        var result = await _sut.ExecuteAsync(request, default);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value!.Items.Should().HaveCount(totalCount);
        result.Value.TotalItems.Should().Be(totalCount);

        _repoMock.Verify(x => x.GetPagedAsync(request.Status, 1, 10, default), Times.Once);
    }

    [Fact]
    public async Task Execute_DeveAjustarPaginacao_QuandoValoresForemMenoresOuIguaisAZero()
    {
        // Arrange
        var request = GetListBeneficiarioRequestMother.PaginaInvalida();
        var (items, totalCount) = BeneficiarioDtoMother.PaginaComUmItem();

        _repoMock.Setup(x => x.GetPagedAsync(It.IsAny<EStatusBeneficiario>(), It.IsAny<int>(), It.IsAny<int>(), default))
                 .ReturnsAsync((items, totalCount));

        // Act
        await _sut.ExecuteAsync(request, default);

        // Assert
        // Verifica se o Use Case corrigiu 0 para 1 na página e 0 para 10 no tamanho
        _repoMock.Verify(x => x.GetPagedAsync(request.Status, 1, 10, default), Times.Once);
    }
    [Fact]
    public async Task Execute_DeveMapearCorretamenteParaResponse()
    {
        // Arrange
        var request = GetListBeneficiarioRequestMother.Padrao();

        var dtoSimulado = BeneficiarioDtoMother.RecuperarValido();

        var listaMockada = (new List<BeneficiarioPageDto> { dtoSimulado }.AsEnumerable(), 1);

        // Ajuste o Setup para usar o Enum correto (EStatusBeneficiario) se necessário
        _repoMock.Setup(x => x.GetPagedAsync(
                    It.IsAny<EStatusBeneficiario>(),
                    It.IsAny<int>(),
                    It.IsAny<int>(),
                    It.IsAny<CancellationToken>()))
                 .ReturnsAsync(listaMockada);

        // Act
        var result = await _sut.ExecuteAsync(request, default);

        // Assert
        result.IsSuccess.Should().BeTrue();
        var itemFormated = result.Value!.Items.First();
        itemFormated.Should().NotBeNull();

    }
}