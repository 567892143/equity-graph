namespace EquityGraph.Api.Features.Companies.GetSupplyChainExposure;

using EquityGraph.Api.Shared.CognoDb;
using Neo4j.Driver;

public class GetSupplyChainExposureQueryHandler
{
    private readonly ICypherReader _reader;

    public GetSupplyChainExposureQueryHandler(ICypherReader reader)
    {
        _reader = reader;
    }

    public async Task<List<SupplyChainPath>> HandleAsync(GetSupplyChainExposureQuery query)
    {
        if (query.MaxHops < 1 || query.MaxHops > 3)
        {
            throw new ArgumentOutOfRangeException(nameof(query.MaxHops), "MaxHops must be between 1 and 3.");
        }

        var cypher = $"MATCH path = (c:Company {{id: $companyId}})<-[:SUPPLIES_TO*1..{query.MaxHops}]-(supplier:Company) " +
                     "RETURN [n IN nodes(path) | {id: n.id, name: n.name}] AS chainNodes, " +
                     "[r IN relationships(path) | r.dependencyPct] AS dependencyPercentages, " +
                     "length(path) AS hops ORDER BY hops";

        return await _reader.ReadAsync(
            cypher,
            new { companyId = query.CompanyId },
            record =>
            {
                var rawNodes = record["chainNodes"].As<List<object>>();
                var chainNodes = rawNodes.Select(item =>
                {
                    if (item is IDictionary<string, object> d)
                    {
                        return new ChainNode(
                            d.TryGetValue("id", out var idVal) ? idVal?.ToString() ?? string.Empty : string.Empty,
                            d.TryGetValue("name", out var nameVal) ? nameVal?.ToString() ?? string.Empty : string.Empty
                        );
                    }
                    if (item is IReadOnlyDictionary<string, object> rd)
                    {
                        return new ChainNode(
                            rd.TryGetValue("id", out var idVal) ? idVal?.ToString() ?? string.Empty : string.Empty,
                            rd.TryGetValue("name", out var nameVal) ? nameVal?.ToString() ?? string.Empty : string.Empty
                        );
                    }
                    return new ChainNode(string.Empty, string.Empty);
                }).ToList();

                var rawDeps = record["dependencyPercentages"].As<List<object>>();
                var dependencyPercentages = rawDeps.Select(d => Convert.ToDouble(d)).ToList();
                var hops = record["hops"].As<int>();

                return new SupplyChainPath(chainNodes, dependencyPercentages, hops);
            }
        );
    }
}
