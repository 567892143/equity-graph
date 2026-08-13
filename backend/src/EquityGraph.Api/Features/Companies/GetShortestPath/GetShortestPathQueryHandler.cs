namespace EquityGraph.Api.Features.Companies.GetShortestPath;

using EquityGraph.Api.Shared.CognoDb;
using Microsoft.Extensions.Logging;
using Neo4j.Driver;

/// <summary>Handles computing the shortest connection path between two companies across any relationship type.</summary>
public class GetShortestPathQueryHandler
{
    private readonly ICypherReader _reader;
    private readonly ILogger<GetShortestPathQueryHandler> _logger;

    /// <summary>Initializes a new instance of GetShortestPathQueryHandler.</summary>
    public GetShortestPathQueryHandler(ICypherReader reader, ILogger<GetShortestPathQueryHandler> logger)
    {
        _reader = reader;
        _logger = logger;
    }

    /// <summary>Executes the query to find the shortest path between two companies within 6 hops.</summary>
    public async Task<ShortestPathResponse?> HandleAsync(GetShortestPathQuery query)
    {
        _logger.LogDebug("Finding shortest path from {FromCompanyId} to {ToCompanyId}", query.FromCompanyId, query.ToCompanyId);

        const string cypher = """
            MATCH (a:Company {id: $fromCompanyId}), (b:Company {id: $toCompanyId})
            MATCH path = shortestPath((a)-[*..6]-(b))
            RETURN [n IN nodes(path) | {id: coalesce(n.id, ''), name: coalesce(n.name, ''), label: labels(n)[0]}] AS pathNodes,
                   [r IN relationships(path) | type(r)] AS relationshipTypes,
                   length(path) AS hops
            """;

        var results = await _reader.ReadAsync(
            cypher,
            new
            {
                fromCompanyId = query.FromCompanyId,
                toCompanyId = query.ToCompanyId
            },
            record =>
            {
                var rawNodes = record["pathNodes"].As<List<object>>();
                var pathNodes = rawNodes.Select(item =>
                {
                    if (item is IDictionary<string, object> d)
                    {
                        return new PathNode(
                            d.TryGetValue("id", out var idVal) ? idVal?.ToString() ?? string.Empty : string.Empty,
                            d.TryGetValue("name", out var nameVal) ? nameVal?.ToString() ?? string.Empty : string.Empty,
                            d.TryGetValue("label", out var labelVal) ? labelVal?.ToString() ?? string.Empty : string.Empty
                        );
                    }
                    if (item is IReadOnlyDictionary<string, object> rd)
                    {
                        return new PathNode(
                            rd.TryGetValue("id", out var idVal) ? idVal?.ToString() ?? string.Empty : string.Empty,
                            rd.TryGetValue("name", out var nameVal) ? nameVal?.ToString() ?? string.Empty : string.Empty,
                            rd.TryGetValue("label", out var labelVal) ? labelVal?.ToString() ?? string.Empty : string.Empty
                        );
                    }
                    return new PathNode(string.Empty, string.Empty, string.Empty);
                }).ToList();

                var rawRelTypes = record["relationshipTypes"].As<List<object>>();
                var relationshipTypes = rawRelTypes.Select(r => r?.ToString() ?? string.Empty).ToList();
                var hops = record["hops"].As<int>();

                return new ShortestPathResponse(pathNodes, relationshipTypes, hops);
            }
        );

        var result = results.FirstOrDefault();
        if (result is null)
        {
            _logger.LogDebug("No path found between {FromCompanyId} and {ToCompanyId}", query.FromCompanyId, query.ToCompanyId);
        }
        else
        {
            _logger.LogDebug("Found shortest path of {Hops} hops between {FromCompanyId} and {ToCompanyId}", result.Hops, query.FromCompanyId, query.ToCompanyId);
        }

        return result;
    }
}
