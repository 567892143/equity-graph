namespace EquityGraph.Api.Features.Companies.GetShortestPath;

/// <summary>Represents a node along a shortest path traversal.</summary>
public record PathNode(
    string Id,
    string Name,
    string Label
);

/// <summary>Represents the shortest connection path between two graph entities.</summary>
public record ShortestPathResponse(
    List<PathNode> Nodes,
    List<string> RelationshipTypes,
    int Hops
);
