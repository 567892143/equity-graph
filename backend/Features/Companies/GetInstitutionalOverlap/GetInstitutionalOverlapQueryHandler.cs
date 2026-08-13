namespace EquityGraph.Api.Features.Companies.GetInstitutionalOverlap;

using EquityGraph.Api.Shared.CognoDb;
using Neo4j.Driver;

public class GetInstitutionalOverlapQueryHandler
{
    private readonly ICypherReader _reader;

    public GetInstitutionalOverlapQueryHandler(ICypherReader reader)
    {
        _reader = reader;
    }

    public async Task<List<InstitutionalOverlapEntry>> HandleAsync(GetInstitutionalOverlapQuery query)
    {
        const string cypher = """
            MATCH (i:Institution)-[r1:HOLDS_STAKE_IN]->(c1:Company {id: $companyIdA})
            MATCH (i)-[r2:HOLDS_STAKE_IN]->(c2:Company {id: $companyIdB})
            RETURN i.id AS institutionId, i.name AS institutionName,
                   r1.stakePct AS stakeInCompanyA, r2.stakePct AS stakeInCompanyB
            ORDER BY i.name
            """;

        return await _reader.ReadAsync(
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
    }
}
