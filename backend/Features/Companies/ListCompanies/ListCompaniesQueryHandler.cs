namespace EquityGraph.Api.Features.Companies.ListCompanies;

using EquityGraph.Api.Shared.CognoDb;
using Neo4j.Driver;

public class ListCompaniesQueryHandler
{
    private readonly ICypherReader _reader;

    public ListCompaniesQueryHandler(ICypherReader reader)
    {
        _reader = reader;
    }

    public async Task<List<CompanySummary>> HandleAsync(ListCompaniesQuery query)
    {
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

        return await _reader.ReadAsync(
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
    }
}
