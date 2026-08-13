namespace EquityGraph.Api.Features.Companies.GetShortestPath;

public record PathNode(
    string Id,
    string Name,
    string Label
);

public record ShortestPathResponse(
    List<PathNode> Nodes,
    List<string> RelationshipTypes,
    int Hops
);
