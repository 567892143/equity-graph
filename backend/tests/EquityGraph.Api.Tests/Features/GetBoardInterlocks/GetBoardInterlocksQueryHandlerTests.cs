namespace EquityGraph.Api.Tests.Features.GetBoardInterlocks;

using EquityGraph.Api.Features.Companies.GetBoardInterlocks;
using EquityGraph.Api.Shared.CognoDb;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Neo4j.Driver;
using Xunit;

public class GetBoardInterlocksQueryHandlerTests
{
    private readonly Mock<ICypherReader> _mockReader;
    private readonly Mock<ILogger<GetBoardInterlocksQueryHandler>> _mockLogger;
    private readonly GetBoardInterlocksQueryHandler _handler;

    public GetBoardInterlocksQueryHandlerTests()
    {
        _mockReader = new Mock<ICypherReader>();
        _mockLogger = new Mock<ILogger<GetBoardInterlocksQueryHandler>>();
        _handler = new GetBoardInterlocksQueryHandler(_mockReader.Object, _mockLogger.Object);
    }

    /// <summary>
    /// Verifies that GetBoardInterlocksQueryHandler returns all overlapping director relationships for a given company.
    /// </summary>
    [Fact]
    public async Task HandleAsync_WhenInterlocksExist_ReturnsBoardInterlocksList()
    {
        // Arrange
        var query = new GetBoardInterlocksQuery("comp-1");
        var expected = new List<BoardInterlock>
        {
            new("person-1", "Natarajan Chandrasekaran", 2016, "comp-3", "Tata Motors Limited"),
            new("person-2", "Keki Mistry", 2018, "comp-5", "HDFC Bank Limited")
        };

        _mockReader
            .Setup(r => r.ReadAsync(
                It.IsAny<string>(),
                It.IsAny<object?>(),
                It.IsAny<Func<IRecord, BoardInterlock>>()))
            .ReturnsAsync(expected);

        // Act
        var result = await _handler.HandleAsync(query);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result.Should().BeEquivalentTo(expected);
    }

    /// <summary>
    /// Verifies that GetBoardInterlocksQueryHandler returns an empty list when no board interlocks exist for the target company.
    /// </summary>
    [Fact]
    public async Task HandleAsync_WhenNoInterlocksExist_ReturnsEmptyList()
    {
        // Arrange
        var query = new GetBoardInterlocksQuery("comp-8");

        _mockReader
            .Setup(r => r.ReadAsync(
                It.IsAny<string>(),
                It.IsAny<object?>(),
                It.IsAny<Func<IRecord, BoardInterlock>>()))
            .ReturnsAsync(new List<BoardInterlock>());

        // Act
        var result = await _handler.HandleAsync(query);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }
}
