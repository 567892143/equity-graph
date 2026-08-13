namespace EquityGraph.Api.Features.Companies.GetSupplyChainExposure;

/// <summary>Query parameters for analyzing upstream supply chain exposure up to a maximum number of hops.</summary>
public record GetSupplyChainExposureQuery(
    string CompanyId,
    int MaxHops = 1
);
