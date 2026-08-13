namespace EquityGraph.Api.Features.Companies.GetShortestPath;

/// <summary>Query parameters for discovering the shortest connection path between two companies.</summary>
public record GetShortestPathQuery(
    string FromCompanyId,
    string ToCompanyId
);
