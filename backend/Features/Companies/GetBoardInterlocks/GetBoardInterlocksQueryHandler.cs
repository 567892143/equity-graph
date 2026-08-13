namespace EquityGraph.Api.Features.Companies.GetBoardInterlocks;

using EquityGraph.Api.Shared.CognoDb;
using Neo4j.Driver;

public class GetBoardInterlocksQueryHandler
{
    private readonly ICypherReader _reader;

    public GetBoardInterlocksQueryHandler(ICypherReader reader)
    {
        _reader = reader;
    }

    public async Task<List<BoardInterlock>> HandleAsync(GetBoardInterlocksQuery query)
    {
        const string cypher = """
            MATCH (c:Company {id: $companyId})<-[r:DIRECTOR_OF]-(p:Person)
            MATCH (p)-[:DIRECTOR_OF]->(other:Company)
            WHERE other.id <> $companyId
            RETURN p.id AS personId, p.name AS personName, r.since AS since,
                   other.id AS otherCompanyId, other.name AS otherCompanyName
            ORDER BY p.name
            """;

        return await _reader.ReadAsync(
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
    }
}
