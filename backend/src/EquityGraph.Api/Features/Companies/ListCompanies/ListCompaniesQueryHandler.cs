namespace EquityGraph.Api.Features.Companies.ListCompanies;

using EquityGraph.Api.Shared.CognoDb;
using Microsoft.Extensions.Logging;
using Neo4j.Driver;

/// <summary>Handles querying and filtering the list of companies.</summary>
public class ListCompaniesQueryHandler
{
    private readonly ICypherReader _reader;
    private readonly ILogger<ListCompaniesQueryHandler> _logger;

    /// <summary>Initializes a new instance of ListCompaniesQueryHandler.</summary>
    public ListCompaniesQueryHandler(ICypherReader reader, ILogger<ListCompaniesQueryHandler> logger)
    {
        _reader = reader;
        _logger = logger;
    }

    /// <summary>Executes the query to fetch filtered companies from the graph database.</summary>
    public async Task<List<CompanySummary>> HandleAsync(ListCompaniesQuery query)
    {
        _logger.LogDebug("Listing companies with search: {Search}, sector: {Sector}", query.Search, query.Sector);

        const string cypher = """
            MATCH (c:Company)
            WHERE ($search IS NULL OR toLower(c.name) CONTAINS toLower($search)
                   OR toLower(c.ticker) CONTAINS toLower($search))
              AND ($sector IS NULL OR c.sector = $sector)
            RETURN c.id AS id, c.name AS name, c.ticker AS ticker,
                   c.sector AS sector, c.marketCap AS marketCap
            ORDER BY c.marketCap DESC
            """;

        var parameters = new
        {
            search = string.IsNullOrWhiteSpace(query.Search) ? null : query.Search,
            sector = string.IsNullOrWhiteSpace(query.Sector) ? null : query.Sector
        };

        var results = await _reader.ReadAsync(
            cypher,
            parameters,
            record => new CompanySummary(
                record["id"].As<string>(),
                record["name"].As<string>(),
                record["ticker"].As<string>(),
                record["sector"].As<string>(),
                record["marketCap"].As<double>()
            )
        );

        _logger.LogDebug("Found {Count} companies matching criteria", results.Count);

        return results;
    }
}
