namespace EquityGraph.Api.Features.Companies.GetInstitutionalOverlap;

using EquityGraph.Api.Shared.CognoDb;
using Microsoft.Extensions.Logging;
using Neo4j.Driver;

/// <summary>Handles querying overlapping institutional shareholders between two companies.</summary>
public class GetInstitutionalOverlapQueryHandler
{
    private readonly ICypherReader _reader;
    private readonly ILogger<GetInstitutionalOverlapQueryHandler> _logger;

    /// <summary>Initializes a new instance of GetInstitutionalOverlapQueryHandler.</summary>
    public GetInstitutionalOverlapQueryHandler(ICypherReader reader, ILogger<GetInstitutionalOverlapQueryHandler> logger)
    {
        _reader = reader;
        _logger = logger;
    }

    /// <summary>Executes the query to fetch institutional ownership overlap between two companies.</summary>
    public async Task<List<InstitutionalOverlapEntry>> HandleAsync(GetInstitutionalOverlapQuery query)
    {
        _logger.LogDebug("Fetching institutional overlap between {CompanyIdA} and {CompanyIdB}", query.CompanyIdA, query.CompanyIdB);

        const string cypher = """
            MATCH (i:Institution)-[r1:HOLDS_STAKE_IN]->(c1:Company {id: $companyIdA})
            MATCH (i)-[r2:HOLDS_STAKE_IN]->(c2:Company {id: $companyIdB})
            RETURN i.id AS institutionId, i.name AS institutionName,
                   r1.stakePct AS stakeInCompanyA, r2.stakePct AS stakeInCompanyB
            ORDER BY i.name
            """;

        var results = await _reader.ReadAsync(
            cypher,
            new
            {
                companyIdA = query.CompanyIdA,
                companyIdB = query.CompanyIdB
            },
            record => new InstitutionalOverlapEntry(
                record["institutionId"].As<string>(),
                record["institutionName"].As<string>(),
                record["stakeInCompanyA"].As<double>(),
                record["stakeInCompanyB"].As<double>()
            )
        );

        _logger.LogDebug("Found {Count} overlapping institutional investors", results.Count);

        return results;
    }
}
