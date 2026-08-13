namespace EquityGraph.Api.Features.Companies.GetBoardInterlocks;

using EquityGraph.Api.Shared.CognoDb;
using Microsoft.Extensions.Logging;
using Neo4j.Driver;

/// <summary>Handles fetching board interlocks where directors serve on multiple company boards.</summary>
public class GetBoardInterlocksQueryHandler
{
    private readonly ICypherReader _reader;
    private readonly ILogger<GetBoardInterlocksQueryHandler> _logger;

    /// <summary>Initializes a new instance of GetBoardInterlocksQueryHandler.</summary>
    public GetBoardInterlocksQueryHandler(ICypherReader reader, ILogger<GetBoardInterlocksQueryHandler> logger)
    {
        _reader = reader;
        _logger = logger;
    }

    /// <summary>Executes the query to fetch interlocking board relationships for a company.</summary>
    public async Task<List<BoardInterlock>> HandleAsync(GetBoardInterlocksQuery query)
    {
        _logger.LogDebug("Fetching board interlocks for company {CompanyId}", query.CompanyId);

        const string cypher = """
            MATCH (c:Company {id: $companyId})<-[r:DIRECTOR_OF]-(p:Person)
            MATCH (p)-[:DIRECTOR_OF]->(other:Company)
            WHERE other.id <> $companyId
            RETURN p.id AS personId, p.name AS personName, r.since AS since,
                   other.id AS otherCompanyId, other.name AS otherCompanyName
            ORDER BY p.name
            """;

        var results = await _reader.ReadAsync(
            cypher,
            new { companyId = query.CompanyId },
            record => new BoardInterlock(
                record["personId"].As<string>(),
                record["personName"].As<string>(),
                record["since"].As<int>(),
                record["otherCompanyId"].As<string>(),
                record["otherCompanyName"].As<string>()
            )
        );

        _logger.LogDebug("Found {Count} board interlocks for company {CompanyId}", results.Count, query.CompanyId);

        return results;
    }
}
