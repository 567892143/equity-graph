namespace EquityGraph.Api.Tests.Features.GetShortestPath;

using EquityGraph.Api.Features.Companies.GetShortestPath;
using EquityGraph.Api.Shared.CognoDb;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Neo4j.Driver;
using Xunit;

public class GetShortestPathQueryHandlerTests
{
    private readonly Mock<ICypherReader> _mockReader;
    private readonly Mock<ILogger<GetShortestPathQueryHandler>> _mockLogger;
    private readonly GetShortestPathQueryHandler _handler;

    public GetShortestPathQueryHandlerTests()
    {
        _mockReader = new Mock<ICypherReader>();
        _mockLogger = new Mock<ILogger<GetShortestPathQueryHandler>>();
        _handler = new GetShortestPathQueryHandler(_mockReader.Object, _mockLogger.Object);
    }

    /// <summary>
    /// Verifies that GetShortestPathQueryHandler returns the shortest path graph response connecting two companies.
    /// </summary>
    [Fact]
    public async Task HandleAsync_WhenPathExists_ReturnsShortestPathResponse()
    {
        // Arrange
        var query = new GetShortestPathQuery("comp-1", "comp-3");
        var expected = new ShortestPathResponse(
            new List<PathNode>
            {
                new("comp-1", "Tata Consultancy Services", "Company"),
                new("person-1", "Natarajan Chandrasekaran", "Person"),
                new("comp-3", "Tata Motors Limited", "Company")
            },
            new List<string> { "DIRECTOR_OF", "DIRECTOR_OF" },
            2
        );

        _mockReader
            .Setup(r => r.ReadAsync(
                It.IsAny<string>(),
                It.IsAny<object?>(),
                It.IsAny<Func<IRecord, ShortestPathResponse>>()))
            .ReturnsAsync(new List<ShortestPathResponse> { expected });

        // Act
        var result = await _handler.HandleAsync(query);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expected);
        result!.Hops.Should().Be(2);
        result.Nodes.Should().HaveCount(3);
        result.RelationshipTypes.Should().HaveCount(2);
    }

    /// <summary>
    /// Verifies that GetShortestPathQueryHandler returns null when no path exists between the specified companies within the hop limit.
    /// </summary>
    [Fact]
    public async Task HandleAsync_WhenNoPathExists_ReturnsNull()
    {
        // Arrange
        var query = new GetShortestPathQuery("comp-1", "comp-999");

        _mockReader
            .Setup(r => r.ReadAsync(
                It.IsAny<string>(),
                It.IsAny<object?>(),
                It.IsAny<Func<IRecord, ShortestPathResponse>>()))
            .ReturnsAsync(new List<ShortestPathResponse>());

        // Act
        var result = await _handler.HandleAsync(query);

        // Assert
        result.Should().BeNull();
    }
}
