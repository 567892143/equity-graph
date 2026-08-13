namespace EquityGraph.Api.Tests.Features.ListCompanies;

using EquityGraph.Api.Features.Companies.ListCompanies;
using EquityGraph.Api.Shared.CognoDb;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Neo4j.Driver;
using Xunit;

public class ListCompaniesQueryHandlerTests
{
    private readonly Mock<ICypherReader> _mockReader;
    private readonly Mock<ILogger<ListCompaniesQueryHandler>> _mockLogger;
    private readonly ListCompaniesQueryHandler _handler;

    public ListCompaniesQueryHandlerTests()
    {
        _mockReader = new Mock<ICypherReader>();
        _mockLogger = new Mock<ILogger<ListCompaniesQueryHandler>>();
        _handler = new ListCompaniesQueryHandler(_mockReader.Object, _mockLogger.Object);
    }

    /// <summary>
    /// Verifies that ListCompaniesQueryHandler returns matching mapped companies when search and sector filters are provided.
    /// </summary>
    [Fact]
    public async Task HandleAsync_WithSearchAndSector_ReturnsMatchingCompanies()
    {
        // Arrange
        var query = new ListCompaniesQuery("Tata", "Automotive");
        var expected = new List<CompanySummary>
        {
            new("comp-3", "Tata Motors Limited", "TATAMOTORS.NS", "Automotive", 42000000000.0)
        };

        _mockReader
            .Setup(r => r.ReadAsync(
                It.IsAny<string>(),
                It.IsAny<object?>(),
                It.IsAny<Func<IRecord, CompanySummary>>()))
            .ReturnsAsync(expected);

        // Act
        var result = await _handler.HandleAsync(query);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(1);
        result.First().Name.Should().Be("Tata Motors Limited");
        result.First().Sector.Should().Be("Automotive");
        _mockReader.Verify(r => r.ReadAsync(
            It.Is<string>(c => c.Contains("MATCH (c:Company)")),
            It.IsAny<object?>(),
            It.IsAny<Func<IRecord, CompanySummary>>()), Times.Once);
    }

    /// <summary>
    /// Verifies that ListCompaniesQueryHandler returns an empty list when no companies match the given search criteria.
    /// </summary>
    [Fact]
    public async Task HandleAsync_WhenNoCompaniesMatch_ReturnsEmptyList()
    {
        // Arrange
        var query = new ListCompaniesQuery("NonExistent", null);

        _mockReader
            .Setup(r => r.ReadAsync(
                It.IsAny<string>(),
                It.IsAny<object?>(),
                It.IsAny<Func<IRecord, CompanySummary>>()))
            .ReturnsAsync(new List<CompanySummary>());

        // Act
        var result = await _handler.HandleAsync(query);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }
}
