namespace EquityGraph.Api.Tests.Features.GetSupplyChainExposure;

using EquityGraph.Api.Features.Companies.GetSupplyChainExposure;
using EquityGraph.Api.Shared.CognoDb;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Neo4j.Driver;
using Xunit;

public class GetSupplyChainExposureQueryHandlerTests
{
    private readonly Mock<ICypherReader> _mockReader;
    private readonly Mock<ILogger<GetSupplyChainExposureQueryHandler>> _mockLogger;
    private readonly GetSupplyChainExposureQueryHandler _handler;

    public GetSupplyChainExposureQueryHandlerTests()
    {
        _mockReader = new Mock<ICypherReader>();
        _mockLogger = new Mock<ILogger<GetSupplyChainExposureQueryHandler>>();
        _handler = new GetSupplyChainExposureQueryHandler(_mockReader.Object, _mockLogger.Object);
    }

    /// <summary>
    /// Verifies that GetSupplyChainExposureQueryHandler throws an ArgumentOutOfRangeException when maxHops is outside the allowable range of 1 to 3 (e.g., 0, 4).
    /// </summary>
    [Theory]
    [InlineData(0)]
    [InlineData(4)]
    [InlineData(-1)]
    [InlineData(5)]
    public async Task HandleAsync_WithInvalidHops_ThrowsArgumentOutOfRangeException(int invalidHops)
    {
        // Arrange
        var query = new GetSupplyChainExposureQuery("comp-1", invalidHops);

        // Act
        var act = () => _handler.HandleAsync(query);

        // Assert
        await act.Should().ThrowAsync<ArgumentOutOfRangeException>()
            .WithParameterName("MaxHops");
    }

    /// <summary>
    /// Verifies that GetSupplyChainExposureQueryHandler succeeds and does not throw for valid maxHops values of 1, 2, and 3.
    /// </summary>
    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public async Task HandleAsync_WithValidHops_DoesNotThrowAndReturnsPaths(int validHops)
    {
        // Arrange
        var query = new GetSupplyChainExposureQuery("comp-1", validHops);
        var expected = new List<SupplyChainPath>
        {
            new(
                new List<ChainNode>
                {
                    new("comp-1", "Tata Consultancy Services"),
                    new("comp-3", "Tata Motors Limited")
                },
                new List<double> { 15.0 },
                1
            )
        };

        _mockReader
            .Setup(r => r.ReadAsync(
                It.IsAny<string>(),
                It.IsAny<object?>(),
                It.IsAny<Func<IRecord, SupplyChainPath>>()))
            .ReturnsAsync(expected);

        // Act
        var result = await _handler.HandleAsync(query);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(1);
        _mockReader.Verify(r => r.ReadAsync(
            It.Is<string>(c => c.Contains($"[:SUPPLIES_TO*1..{validHops}]")),
            It.IsAny<object?>(),
            It.IsAny<Func<IRecord, SupplyChainPath>>()), Times.Once);
    }
}
