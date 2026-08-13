namespace EquityGraph.Api.Features.Companies.GetSupplyChainExposure;

public record ChainNode(
    string Id,
    string Name
);

public record SupplyChainPath(
    List<ChainNode> Nodes,
    List<double> DependencyPercentages,
    int Hops
);
