using CRC.Api.Repository;
using CRC.Domain.Entities;
using Moq;

namespace CRC.Tests;

public class MoradorBonusRepositoryTests
{
    private readonly Mock<IMoradorBonusRepository> _mockRepository;

    public MoradorBonusRepositoryTests()
    {
        _mockRepository = new Mock<IMoradorBonusRepository>();
    }
    

    [Fact]
    public async Task GetByIdAsync_ShouldReturnCorrectEntity()
    {
        // Arrange
        var id = 1;
        var expectedEntity = new MoradorBonus { Id = id };

        _mockRepository
            .Setup(repo => repo.GetByIdAsync(id))
            .ReturnsAsync(expectedEntity);

        // Act
        var result = await _mockRepository.Object.GetByIdAsync(id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedEntity, result);
    }

    [Fact]
    public async Task GetByMoradorIdAsync_ShouldReturnCorrectData()
    {
        // Arrange
        var moradorId = 10;
        var expectedData = new List<MoradorBonus>
        {
            new MoradorBonus { Id = 1, IdMorador = moradorId },
            new MoradorBonus { Id = 2, IdMorador = moradorId }
        };

        _mockRepository
            .Setup(repo => repo.GetByMoradorIdAsync(moradorId))
            .ReturnsAsync(expectedData);

        // Act
        var result = await _mockRepository.Object.GetByMoradorIdAsync(moradorId);

        // Assert
        Assert.Equal(expectedData.Count, result.Count());
        Assert.All(result, mb => Assert.Equal(moradorId, mb.IdMorador));
    }

    [Fact]
    public async Task GetByBonusIdAsync_ShouldReturnCorrectData()
    {
        // Arrange
        var bonusId = 5;
        var expectedData = new List<MoradorBonus>
        {
            new MoradorBonus { Id = 1, IdBonus = bonusId },
            new MoradorBonus { Id = 2, IdBonus = bonusId }
        };

        _mockRepository
            .Setup(repo => repo.GetByBonusIdAsync(bonusId))
            .ReturnsAsync(expectedData);

        // Act
        var result = await _mockRepository.Object.GetByBonusIdAsync(bonusId);

        // Assert
        Assert.Equal(expectedData.Count, result.Count());
        Assert.All(result, mb => Assert.Equal(bonusId, mb.IdBonus));
    }

    [Fact]
    public async Task GetByMoradorIdAndBonusIdAsync_ShouldReturnCorrectEntity()
    {
        // Arrange
        var moradorId = 10;
        var bonusId = 5;
        var expectedEntity = new MoradorBonus { Id = 1, IdMorador = moradorId, IdBonus = bonusId };

        _mockRepository
            .Setup(repo => repo.GetByMoradorIdAndBonusIdAsync(moradorId, bonusId))
            .ReturnsAsync(expectedEntity);

        // Act
        var result = await _mockRepository.Object.GetByMoradorIdAndBonusIdAsync(moradorId, bonusId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedEntity, result);
    }

    [Fact]
    public async Task GetAvaliableByIdAsync_ShouldReturnCorrectSum()
    {
        // Arrange
        var bonusId = 1;
        var expectedSum = 30;

        _mockRepository
            .Setup(repo => repo.GetAvaliableByIdAsync(bonusId))
            .ReturnsAsync(expectedSum);

        // Act
        var result = await _mockRepository.Object.GetAvaliableByIdAsync(bonusId);

        // Assert
        Assert.Equal(expectedSum, result);
    }
}
