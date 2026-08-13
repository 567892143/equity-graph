namespace EquityGraph.Api.Tests.Features.GetCompanyDetail;

using EquityGraph.Api.Features.Companies.GetCompanyDetail;
using EquityGraph.Api.Shared.CognoDb;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Neo4j.Driver;
using Xunit;

public class GetCompanyDetailQueryHandlerTests
{
    private readonly Mock<ICypherReader> _mockReader;
    private readonly Mock<ILogger<GetCompanyDetailQueryHandler>> _mockLogger;
    private readonly GetCompanyDetailQueryHandler _handler;

    public GetCompanyDetailQueryHandlerTests()
    {
        _mockReader = new Mock<ICypherReader>();
        _mockLogger = new Mock<ILogger<GetCompanyDetailQueryHandler>>();
        _handler = new GetCompanyDetailQueryHandler(_mockReader.Object, _mockLogger.Object);
    }

    /// <summary>
    /// Verifies that GetCompanyDetailQueryHandler returns the company detail response when a company exists in the database.
    /// </summary>
    [Fact]
    public async Task HandleAsync_WhenCompanyExists_ReturnsCompanyDetail()
    {
        // Arrange
        var query = new GetCompanyDetailQuery("comp-1");
        var expected = new CompanyDetailResponse(
            "comp-1",
            "Tata Consultancy Services",
            "TCS.NS",
            "Information Technology",
            160000000000.0,
            3,
            18.0,
            3
        );

        _mockReader
            .Setup(r => r.ReadAsync(
                It.IsAny<string>(),
                It.IsAny<object?>(),
                It.IsAny<Func<IRecord, CompanyDetailResponse>>()))
            .ReturnsAsync(new List<CompanyDetailResponse> { expected });

        // Act
        var result = await _handler.HandleAsync(query);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expected);
        result!.DirectorCount.Should().Be(3);
        result.MaxSupplyDependencyPct.Should().Be(18.0);
        result.InstitutionCount.Should().Be(3);
    }

    /// <summary>
    /// Verifies that GetCompanyDetailQueryHandler returns null when the company does not exist.
    /// </summary>
    [Fact]
    public async Task HandleAsync_WhenCompanyNotFound_ReturnsNull()
    {
        // Arrange
        var query = new GetCompanyDetailQuery("comp-999");

        _mockReader
            .Setup(r => r.ReadAsync(
                It.IsAny<string>(),
                It.IsAny<object?>(),
                It.IsAny<Func<IRecord, CompanyDetailResponse>>()))
            .ReturnsAsync(new List<CompanyDetailResponse>());

        // Act
        var result = await _handler.HandleAsync(query);

        // Assert
        result.Should().BeNull();
    }
}
