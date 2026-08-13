namespace EquityGraph.Api.Features.Companies.GetSupplyChainExposure;

public record GetSupplyChainExposureQuery(
    string CompanyId,
    int MaxHops = 1
);
