namespace EquityGraph.Api.Tests.Features.GetInstitutionalOverlap;

using EquityGraph.Api.Features.Companies.GetInstitutionalOverlap;
using EquityGraph.Api.Shared.CognoDb;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Neo4j.Driver;
using Xunit;

public class GetInstitutionalOverlapQueryHandlerTests
{
    private readonly Mock<ICypherReader> _mockReader;
    private readonly Mock<ILogger<GetInstitutionalOverlapQueryHandler>> _mockLogger;
    private readonly GetInstitutionalOverlapQueryHandler _handler;

    public GetInstitutionalOverlapQueryHandlerTests()
    {
        _mockReader = new Mock<ICypherReader>();
        _mockLogger = new Mock<ILogger<GetInstitutionalOverlapQueryHandler>>();
        _handler = new GetInstitutionalOverlapQueryHandler(_mockReader.Object, _mockLogger.Object);
    }

    /// <summary>
    /// Verifies that GetInstitutionalOverlapQueryHandler returns institutional investors with common holdings across two companies.
    /// </summary>
    [Fact]
    public async Task HandleAsync_WhenOverlapExists_ReturnsOverlapEntries()
    {
        // Arrange
        var query = new GetInstitutionalOverlapQuery("comp-1", "comp-2");
        var expected = new List<InstitutionalOverlapEntry>
        {
            new("inst-3", "BlackRock Inc.", 3.2, 4.6),
            new("inst-1", "Life Insurance Corporation of India", 4.8, 7.2),
            new("inst-2", "Vanguard Group", 3.5, 4.1)
        };

        _mockReader
            .Setup(r => r.ReadAsync(
                It.IsAny<string>(),
                It.IsAny<object?>(),
                It.IsAny<Func<IRecord, InstitutionalOverlapEntry>>()))
            .ReturnsAsync(expected);

        // Act
        var result = await _handler.HandleAsync(query);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(3);
        result.Should().BeEquivalentTo(expected);
    }

    /// <summary>
    /// Verifies that GetInstitutionalOverlapQueryHandler returns an empty list when no common institutional investors exist.
    /// </summary>
    [Fact]
    public async Task HandleAsync_WhenNoOverlapExists_ReturnsEmptyList()
    {
        // Arrange
        var query = new GetInstitutionalOverlapQuery("comp-7", "comp-8");

        _mockReader
            .Setup(r => r.ReadAsync(
                It.IsAny<string>(),
                It.IsAny<object?>(),
                It.IsAny<Func<IRecord, InstitutionalOverlapEntry>>()))
            .ReturnsAsync(new List<InstitutionalOverlapEntry>());

        // Act
        var result = await _handler.HandleAsync(query);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }
}
