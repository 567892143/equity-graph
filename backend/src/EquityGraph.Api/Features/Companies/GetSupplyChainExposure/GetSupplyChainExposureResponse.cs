namespace EquityGraph.Api.Features.Companies.GetSupplyChainExposure;

/// <summary>Represents a company node in a supply chain dependency path.</summary>
public record ChainNode(
    string Id,
    string Name
);

/// <summary>Represents a directional supply chain dependency path across multiple hops.</summary>
public record SupplyChainPath(
    List<ChainNode> Nodes,
    List<double> DependencyPercentages,
    int Hops
);
