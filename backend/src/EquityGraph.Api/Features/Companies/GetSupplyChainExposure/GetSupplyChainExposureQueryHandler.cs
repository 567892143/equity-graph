namespace EquityGraph.Api.Features.Companies.GetSupplyChainExposure;

using EquityGraph.Api.Shared.CognoDb;
using Microsoft.Extensions.Logging;
using Neo4j.Driver;

/// <summary>Handles traversing and quantifying multi-hop supply chain dependency risks.</summary>
public class GetSupplyChainExposureQueryHandler
{
    private readonly ICypherReader _reader;
    private readonly ILogger<GetSupplyChainExposureQueryHandler> _logger;

    /// <summary>Initializes a new instance of GetSupplyChainExposureQueryHandler.</summary>
    public GetSupplyChainExposureQueryHandler(ICypherReader reader, ILogger<GetSupplyChainExposureQueryHandler> logger)
    {
        _reader = reader;
        _logger = logger;
    }

    /// <summary>Executes the query to fetch supply chain paths up to the validated hop limit.</summary>
    public async Task<List<SupplyChainPath>> HandleAsync(GetSupplyChainExposureQuery query)
    {
        _logger.LogDebug("Fetching supply chain exposure for company {CompanyId} with maxHops {MaxHops}", query.CompanyId, query.MaxHops);

        if (query.MaxHops < 1 || query.MaxHops > 3)
        {
            throw new ArgumentOutOfRangeException(nameof(query.MaxHops), "MaxHops must be between 1 and 3.");
        }

        var cypher = $"MATCH path = (c:Company {{id: $companyId}})<-[:SUPPLIES_TO*1..{query.MaxHops}]-(supplier:Company) " +
                     "RETURN [n IN nodes(path) | {id: n.id, name: n.name}] AS chainNodes, " +
                     "[r IN relationships(path) | r.dependencyPct] AS dependencyPercentages, " +
                     "length(path) AS hops ORDER BY hops";

        var results = await _reader.ReadAsync(
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

        _logger.LogDebug("Found {Count} supply chain exposure paths for company {CompanyId}", results.Count, query.CompanyId);

        return results;
    }
}
