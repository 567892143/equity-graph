namespace EquityGraph.Api.Features.Companies.GetCompanyDetail;

using EquityGraph.Api.Shared.CognoDb;
using Microsoft.Extensions.Logging;
using Neo4j.Driver;

/// <summary>Handles querying detailed company metadata and aggregated relationships.</summary>
public class GetCompanyDetailQueryHandler
{
    private readonly ICypherReader _reader;
    private readonly ILogger<GetCompanyDetailQueryHandler> _logger;

    /// <summary>Initializes a new instance of GetCompanyDetailQueryHandler.</summary>
    public GetCompanyDetailQueryHandler(ICypherReader reader, ILogger<GetCompanyDetailQueryHandler> logger)
    {
        _reader = reader;
        _logger = logger;
    }

    /// <summary>Executes the query to fetch detailed company profile and graph counts.</summary>
    public async Task<CompanyDetailResponse?> HandleAsync(GetCompanyDetailQuery query)
    {
        _logger.LogDebug("Fetching company details for company {CompanyId}", query.CompanyId);

        const string cypher = """
            MATCH (c:Company {id: $companyId})
            OPTIONAL MATCH (p:Person)-[:DIRECTOR_OF]->(c)
            WITH c, count(DISTINCT p) AS directorCount
            OPTIONAL MATCH (c)-[s:SUPPLIES_TO]->(:Company)
            WITH c, directorCount, max(s.dependencyPct) AS maxSupplyDependency
            OPTIONAL MATCH (i:Institution)-[:HOLDS_STAKE_IN]->(c)
            RETURN c.id AS id, c.name AS name, c.ticker AS ticker, c.sector AS sector,
                   c.marketCap AS marketCap, directorCount,
                   coalesce(maxSupplyDependency, 0.0) AS maxSupplyDependency,
                   count(DISTINCT i) AS institutionCount
            """;

        var results = await _reader.ReadAsync(
            cypher,
            new { companyId = query.CompanyId },
            record => new CompanyDetailResponse(
                record["id"].As<string>(),
                record["name"].As<string>(),
                record["ticker"].As<string>(),
                record["sector"].As<string>(),
                record["marketCap"].As<double>(),
                record["directorCount"].As<int>(),
                record["maxSupplyDependency"].As<double>(),
                record["institutionCount"].As<int>()
            )
        );

        var result = results.FirstOrDefault();
        if (result is null)
        {
            _logger.LogDebug("No company found for {CompanyId}", query.CompanyId);
        }
        else
        {
            _logger.LogDebug("Found company details for {CompanyId}", query.CompanyId);
        }

        return result;
    }
}
